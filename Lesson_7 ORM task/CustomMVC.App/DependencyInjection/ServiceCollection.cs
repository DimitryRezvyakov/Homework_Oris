using CustomMVC.App.MVC.Controllers.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CustomMVC.App.DependencyInjection
{
    public enum Scope
    {
        Scoped,
        Transient,
        Singleton,
        NotImplemented
    }

    public class ServiceCollection : IServiceCollection
    {
        public Dictionary<Type, Type> Singleton { get; } = new();
        public Dictionary<Type, Type> Transient { get; } = new();
        public Dictionary<Type, Type> Scoped { get;  } = new();

        public Dictionary<Type, object> Scope = new();
        public Dictionary<Type, object> Singletons = new();
        public Dictionary<Type, object[]?> SettedParameters = new();

        public static ServiceCollection Instance;
        public ServiceCollection() { Instance = this; } // for creating purpose only

        public void ClearScope()
        {
            Scope.Clear();
        }
        public ControllerBase GetController(Type controllerType, object[]? args = null)
        {
            var method = typeof(ServiceCollection)
                        .GetMethod(nameof(CreateInstance),
                         BindingFlags.NonPublic | BindingFlags.Instance)!;

            var invoker = method?.MakeGenericMethod(controllerType);

            var controller = invoker?.Invoke(this, new object[] { args });

            return (ControllerBase)controller!;
        }

        public T GetService<T>(object[]? parameters = null)
        {
            if (HasService(typeof(T), out var implType, out var scope ))
            {
                if (scope == DependencyInjection.Scope.Scoped)
                {
                    if (Scope.ContainsKey(typeof(T)))
                        return (T)Scope[typeof(T)];

                    var method = typeof(ServiceCollection)
                        .GetMethod(nameof(CreateInstance),
                         BindingFlags.NonPublic | BindingFlags.Instance)!
                        .MakeGenericMethod(implType!);

                    var instance = method.Invoke(this, new object[] { parameters });

                    Scope.Add(typeof(T), instance!);

                    return (T)instance!;
                }

                else if (scope == DependencyInjection.Scope.Singleton)
                {
                    if (Singletons.ContainsKey(typeof(T)))
                        return (T)Singletons[typeof(T)];

                    else
                    {
                        var method = typeof(ServiceCollection)
                            .GetMethod(nameof(CreateInstance),
                             BindingFlags.NonPublic | BindingFlags.Instance)!
                            .MakeGenericMethod(implType!);

                        var instance = method.Invoke(this, new object[] { parameters });

                        Singletons.Add(typeof(T), instance!);

                        return (T)instance!;
                    }
                        
                }
                else if (scope == DependencyInjection.Scope.Transient)
                {
                    var method = typeof(ServiceCollection)
                        .GetMethod(nameof(CreateInstance),
                         BindingFlags.NonPublic | BindingFlags.Instance)!
                        .MakeGenericMethod(implType!);

                    var instance = method.Invoke(this, new object[] { parameters });

                    return (T)instance!;
                }
            }

            throw new InvalidOperationException($"Can`t find service {typeof(T)}");
        }

        private T CreateInstance<T>(object[]? data)
        {
            var constructors = typeof(T).GetConstructors();

            if (constructors.Length == 0)
                throw new InvalidOperationException($"{typeof(T)} не имеет публичных конструкторов");

            SettedParameters.TryGetValue(typeof(T), out var settedData);

            var ctor = constructors.OrderByDescending(c => c.GetParameters().Length).FirstOrDefault();

            var parameters = ctor!.GetParameters();

            List<object> values = new List<object>();

            foreach (var param in parameters)
            {

                if (HasService(param.ParameterType, out var implimentationType, out var scope))
                {
                    var method = typeof(ServiceCollection)
                        .GetMethod(nameof(GetService))!
                        .MakeGenericMethod(param.ParameterType);

                    var instance = method.Invoke(this, new object[] { data });

                    values.Add(instance!);

                    continue;
                }

                var p = data?.FirstOrDefault(p => p.GetType() == param.ParameterType) ??
                    settedData?.FirstOrDefault(p => p.GetType() == param.ParameterType);

                values.Add(p);
            }

            return (T)Activator.CreateInstance(typeof(T), values.ToArray())!;
        }

        private bool HasService(Type serviceType, out Type? implimentationType, out Scope scope)
        {
            if (Singleton.TryGetValue(serviceType, out implimentationType))
            {
                scope = DependencyInjection.Scope.Singleton;
                return true;
            }

            else if (Transient.TryGetValue(serviceType, out implimentationType))
            {
                scope = DependencyInjection.Scope.Transient;
                return true;
            }
            else if (Scoped.TryGetValue(serviceType, out implimentationType))
            {
                scope = DependencyInjection.Scope.Scoped;
                return true;
            }

            implimentationType = null;
            scope = DependencyInjection.Scope.NotImplemented;
            return false;
        } 
    }
}

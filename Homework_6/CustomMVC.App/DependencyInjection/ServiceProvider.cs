using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomMVC.App.MVC.Controllers.Common.Entities;
using Shared.Interfaces;
namespace CustomMVC.App.DependencyInjection
{
    public class ServiceProvider : Shared.Interfaces.IServiceProviderCustom
    {
        public ServiceCollection Services { get; } = new();

        private static ServiceProvider? _instance;
        private static object _instanceLock = new object();

        public ServiceProvider()
        {
            //To avoid circular dependency
            this.Services.Singletons.Add(typeof(IServiceProviderCustom), this);
            this.AddSingleton<IServiceProviderCustom, ServiceProvider>();

            //this.AddKnowingServices();
        }

        public static ServiceProvider GetInstance()
        {
            lock (_instanceLock)
            {
                if (_instance == null) _instance = new ServiceProvider();

                return _instance;
            }
        }

        public void ClearScope()
        {
            Services.ClearScope();
        }

        public void AddScoped<TInterface, TImplimentation>(object[]? parameters = null) where TImplimentation : class, TInterface
        {
            Services.Scoped.Add(typeof(TInterface), typeof(TImplimentation));

            if (parameters != null)
                Services.SettedParameters.Add(typeof(TImplimentation), parameters);
        }

        public void AddSingleton<TInterface, TImplimentation>(object[]? parameters = null) where TImplimentation : class, TInterface
        {
            Services.Singleton.Add(typeof(TInterface), typeof(TImplimentation));

            if (parameters != null)
                Services.SettedParameters.Add(typeof(TImplimentation), parameters);
        }

        public void AddTransient<TInterface, TImplimentation>(object[]? parameters = null) where TImplimentation : class, TInterface
        {
            Services.Transient.Add(typeof(TInterface), typeof(TImplimentation));

            if (parameters != null)
                Services.SettedParameters.Add(typeof(TImplimentation), parameters);
        }

        public ControllerBase GetController(Type controllerType, object[]? args = null)
        {
            return Services.GetController(controllerType, args);
        }

        public T GetService<T>(object[]? parameters = null)
        {
            return Services.GetService<T>(parameters);
        }
    }
}

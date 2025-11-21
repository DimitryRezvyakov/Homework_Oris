using Mediator.Interfaces;
using Mediator.Options;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Providers
{
    public class RequestHandlerProvider : IRequestHandlerProvider
    {
        /// <summary>
        /// Storing all pairs like request -> handler
        /// </summary>
        private readonly Dictionary<Type, Type> _handlers = new Dictionary<Type, Type>();
        private readonly IServiceProviderCustom _serviceProvider;
        private readonly IMediatorOptions _mediatorOptions;

        public RequestHandlerProvider(IServiceProviderCustom serviceProvider, IMediatorOptions mediatorOptions)
        {
            _serviceProvider = serviceProvider;
            _mediatorOptions = mediatorOptions;

            ConfigureHandlers(_mediatorOptions.Assemblies);
        }

        /// <summary>
        /// Gets request handler
        /// </summary>
        /// <param name="request">Request type</param>
        /// <returns>Returns request handler instance</returns>
        public object? Get(Type request) 
        {
            var method = _serviceProvider.GetType().GetMethod(nameof(IServiceProviderCustom.GetService));

            var handlerType = _handlers.TryGetValue(request, out var handler);

            if (!handlerType)
                return null;

            var incoker = method?.MakeGenericMethod(handler!);

            return incoker?.Invoke(_serviceProvider, new object[] { null });
        }

        public void ConfigureHandlers(Assembly[] assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                //All requests in this assembly
                var requests = assembly.GetTypes()
                    .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>)) ||
                    t.IsAssignableTo(typeof(IRequest)))
                    .ToList();

                //All handlers in this assembly
                var handlers = assembly.GetTypes()
                    .Where(t => !t.IsAbstract &&
                    t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
                        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)
                        )
                    )
                    .ToList();
                
                //Adding handlers to service collection
                foreach (var handlerType  in handlers)
                {
                    var handlerInterface = handlerType.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
                        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

                    var method = _serviceProvider.GetType().GetMethod(nameof(_serviceProvider.AddTransient));

                    var invoker = method?.MakeGenericMethod(handlerInterface!, handlerType);

                    invoker?.Invoke(_serviceProvider, new object[] {null});
                }

                //Storing all pairs like request -> handler
                foreach (var requset in requests)
               {
                    var handlerType = handlers
                        .FirstOrDefault(h => h.GetInterfaces()
                        .Any(i => i.GetGenericArguments()[0] == requset));

                    var handler = handlerType?
                        .GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

                    if (handler != null)
                    {
                        _handlers.Add(requset, handler);
                    }
                }
            }
        }
    }
}

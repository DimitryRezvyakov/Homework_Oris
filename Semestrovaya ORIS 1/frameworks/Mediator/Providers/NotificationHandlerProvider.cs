using Mediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Shared.Interfaces;

namespace Mediator.Providers
{
    public class NotificationHandlerProvider : INotificationHandlerProvider
    {
        /// <summary>
        /// Store all request -> handler pairs
        /// </summary>
        private static readonly Dictionary<Type, Type> _handlers = new();
        private readonly IServiceProviderCustom _serviceProvider;
        private readonly IMediatorOptions _mediatorOptions;

        public NotificationHandlerProvider(IServiceProviderCustom sp, IMediatorOptions opt)
        {
            _serviceProvider = sp;
            _mediatorOptions = opt;

            ConfigureAllHandlers(opt.Assemblies);
        }

        /// <summary>
        /// Getting request handler
        /// </summary>
        /// <param name="type">Request type</param>
        /// <returns>Returns the request handler instance</returns>
        public object? Get(Type type)
        {
            var method = _serviceProvider.GetType().GetMethod(nameof(IServiceProviderCustom.GetService));

            var handlerType = _handlers.TryGetValue(type, out var handler);

            if (!handlerType)
                return null;

            var incoker = method?.MakeGenericMethod(handler!);

            return incoker?.Invoke(_serviceProvider, new object[] { null });
        }

        /// <summary>
        /// Getting all handlers and requests
        /// </summary>
        /// <param name="assemblies"></param>
        private void ConfigureAllHandlers(Assembly[] assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                //All requests in this assembly
                var notifications = assembly.GetTypes()
                    .Where(t => t.GetInterfaces().Contains(typeof(INotification)))
                    .ToList();

                //All handlers in this assembly
                var notificationHandlers = assembly.GetTypes()
                    .Where(
                    t => !t.IsAbstract &&
                    t.GetInterfaces()
                        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>)
                        )
                    )
                    .ToList();

                //Adding handlers to server collection
                foreach (Type handlerType in notificationHandlers)
                {
                    var handlerInterface = handlerType.GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>));

                    var method = _serviceProvider.GetType().GetMethod(nameof(_serviceProvider.AddTransient));

                    var invoker = method?.MakeGenericMethod(handlerInterface!, handlerType);

                    invoker?.Invoke(_serviceProvider, new object[] { null });
                }

                //Adding pairs request -> handler
                foreach (var notification in notifications)
                {
                    var handlerType = notificationHandlers
                        .FirstOrDefault(h => h.GetInterfaces()
                        .Any(i => i.GetGenericArguments()[0] == notification));

                    var handler = handlerType?.GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>));

                    if (handler == null) return;

                    _handlers.Add(notification, handler);
                }
            }
        }
    }
}

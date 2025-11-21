using Shared.Interfaces;
using Mediator.Interfaces;
using Mediator.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Mediator.Providers;

namespace Mediator.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void UseMediator(this IServiceProviderCustom serviceProvider, Action<IMediatorOptions>? opt = null)
        {
            serviceProvider.AddSingleton<IMediatorOptions, MediatorOptions>(new object[] { opt });
            serviceProvider.AddSingleton<INotificationHandlerProvider, NotificationHandlerProvider>();
            serviceProvider.AddSingleton<IRequestHandlerProvider, RequestHandlerProvider>();
            serviceProvider.AddSingleton<IMediator, Mediator>();
        }
    }
}

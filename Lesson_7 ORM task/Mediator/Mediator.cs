using Mediator.Exceptions;
using Mediator.Interfaces;
using Mediator.Options;
using Mediator.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator
{
    public class Mediator : IMediator
    {
        private IMediatorOptions _options;
        private readonly IRequestHandlerProvider _handlerProvider;
        private readonly INotificationHandlerProvider _notificationHandlerProvider;

        public Mediator(IRequestHandlerProvider handlerProvider, INotificationHandlerProvider notificationHandlerProvider, 
            IMediatorOptions options)
        {
            _options = options;
            _handlerProvider = handlerProvider;
            _notificationHandlerProvider = notificationHandlerProvider;
        }

        public async Task Publish(INotification notification, CancellationToken ct)
        {
            var handler = _notificationHandlerProvider.Get(notification.GetType());

            if (handler != null)
            {
                var handlermethod = handler.GetType().GetMethod("Handle");

                await (Task)handlermethod?.Invoke(handler, new object[] { notification, ct })!;
            }

            else
            {
                if (_options.ThrowIfNotFound)
                    throw new NotificationHandlerNotFoundException(notification.GetType());
            }

        }

        public async Task Send(IRequest request, CancellationToken ct)
        {
            var handler = _handlerProvider.Get(request.GetType());

            if (handler != null)
            {
                var handlermethod = handler.GetType().GetMethod("Handle");

                await (Task)handlermethod?.Invoke(handler, new object[] { request, ct })!;
            }

            else
            {
                if (_options.ThrowIfNotFound)
                    throw new RequestHandlerNotFoundException(request.GetType());
            }
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
        {
            var handlerType = _handlerProvider.Get(request.GetType());

            if (handlerType != null)
            {
                var handlermethod = handlerType.GetType().GetMethod("Handle");

                return await (Task<TResponse>)handlermethod?.Invoke(handlerType, new object[] { request, ct })!;
            }

            else
            {
                if (_options.ThrowIfNotFound)
                    throw new RequestHandlerNotFoundException(request.GetType());

                return await Task.FromResult<TResponse>(default);
            }
        }
    }
}

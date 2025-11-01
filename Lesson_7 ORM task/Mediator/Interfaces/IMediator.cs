using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Interfaces
{
    /// <summary>
    /// Mediator interface
    /// </summary>
    public interface IMediator
    {
        /// <summary>
        /// Sending a command with returns TResponse
        /// </summary>
        /// <typeparam name="TResponse">Response type</typeparam>
        /// <param name="request">Command instance</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Task<TResponse></returns>
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken ct);

        /// <summary>
        /// Sending a command witch returns nothing
        /// </summary>
        /// <param name="request">Command instance</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Task</returns>
        public Task Send(IRequest request, CancellationToken ct);

        /// <summary>
        /// Publish event
        /// </summary>
        /// <param name="notification">Event instance</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Task</returns>
        public Task Publish(INotification notification, CancellationToken ct);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Interfaces
{
    /// <summary>
    /// Metadata
    /// </summary>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>, new()
    {
        public Task<TResponse> Handle(TRequest request, CancellationToken cts);
    }

    /// <summary>
    /// Metadata
    /// </summary>
    /// <typeparam name="TRequest">Request type</typeparam>
    public interface IRequestHandler<TRequest> where TRequest : IRequest, new()
    {
        public Task Handle(TRequest request, CancellationToken cts);
    }
}

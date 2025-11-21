using CustomMVC.App.Core.Middleware;
using CustomMVC.App.Core.Routing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Core.Routing
{
    public class RouteEndpointBuilder : EndpointBuilder
    {

        private string _pattern;
        private RequestDelegate _handler;
        private int _order;

        public RouteEndpointBuilder(string pattern, RequestDelegate? handler, int order)
        {
            ArgumentNullException.ThrowIfNull(handler);
            _pattern = pattern;
            _handler = handler;
            _order = order;
        }

        /// <summary>
        /// Builds the route
        /// </summary>
        /// <returns>RouteEndpoint</returns>
        public override RouteEndpoint Build()
        {
            return new RouteEndpoint(_pattern, _handler, _order, CreateRouteMetadata(Metadata));
        }

        /// <summary>
        /// Creating a metadata for route
        /// </summary>
        /// <param name="metadata">Metadata</param>
        /// <returns>RouteEndpointMetadata</returns>
        private RouteEndpointMetadata CreateRouteMetadata(List<object> metadata)
        {
            return new RouteEndpointMetadata(metadata);
        }
    }
}

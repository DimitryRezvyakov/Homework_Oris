using CustomMVC.App.Core.Middleware;
using CustomMVC.App.Core.Routing.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Core.Routing
{
    public class RouteEndpoint : Endpoint
    {
        /// <summary>
        /// Route pattern
        /// </summary>
        public RoutePattern RoutePattern { get; set; }

        /// <summary>
        /// Handler for this route
        /// </summary>
        public RequestDelegate RequestDelegate { get; set; }

        /// <summary>
        /// Order for this route
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Route metadata
        /// </summary>
        public RouteEndpointMetadata Metadata { get; set; }

        /// <summary>
        /// Route parameters
        /// </summary>
        public NameValueCollection Params { get; set; } = new();

        public RouteEndpoint(string routePattern, RequestDelegate requestDelegate, int order, RouteEndpointMetadata? metadata = null)
        {
            ArgumentNullException.ThrowIfNull(routePattern);
            ArgumentNullException.ThrowIfNull(requestDelegate);

            RoutePattern = new RoutePattern("default", routePattern);
            RequestDelegate = requestDelegate;
            Order = order;
            Metadata = metadata ?? new RouteEndpointMetadata();
        }

        public RouteEndpoint(RoutePattern routePattern, RequestDelegate requestDelegate, int order, RouteEndpointMetadata? metadata = null)
        {
            ArgumentNullException.ThrowIfNull(routePattern);
            ArgumentNullException.ThrowIfNull(requestDelegate);

            RoutePattern = routePattern;
            RequestDelegate = requestDelegate;
            Order = order;
            Metadata = metadata ?? new RouteEndpointMetadata();
        }
    }
}

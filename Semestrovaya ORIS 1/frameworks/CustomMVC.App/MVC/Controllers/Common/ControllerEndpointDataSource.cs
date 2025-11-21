using CustomMVC.App.Core.Routing;
using CustomMVC.App.Core.Routing.Common;
using CustomMVC.App.MVC.Controllers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Common
{
    public class ControllerEndpointDataSource : EndpointDataSource
    {
        private static ControllerEndpointDataSource? _instance;
        private static object lockObj = new object();
        private List<RouteEndpoint>? _endpoints; 
        public List<ControllerRouteEnpointBuilder> _endpointBuilders = new();
        public static ControllerEndpointDataSource Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (_instance == null) _instance = new ControllerEndpointDataSource();
                    return _instance;
                }
            }
        }

        public void Add(ControllerRouteEnpointBuilder builder)
        {
            _endpointBuilders.Add(builder);
        }

        public override List<RouteEndpoint> Endpoints => _endpoints ??=
            _endpointBuilders.Select(b => b.Build()).ToList();
    }
}

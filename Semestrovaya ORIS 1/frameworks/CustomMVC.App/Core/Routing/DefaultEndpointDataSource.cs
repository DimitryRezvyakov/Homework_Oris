using CustomMVC.App.Core.Routing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Core.Routing
{
    public class DefaultEndpointDataSource : EndpointDataSource
    {
        private static DefaultEndpointDataSource? _instance;
        private static object lockObj = new object();
        private List<RouteEndpoint>? _endpoints;
        public List<RouteEndpointBuilder> _endpointBuilders = new();
        public static DefaultEndpointDataSource Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (_instance == null) _instance = new DefaultEndpointDataSource();
                    return _instance;
                }
            }
        }

        public void Add(RouteEndpointBuilder builder)
        {
            _endpointBuilders.Add(builder);
        }

        public override List<RouteEndpoint> Endpoints => _endpoints ??= _endpointBuilders.Select(b => b.Build()).ToList();
    }
}

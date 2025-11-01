using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Core.Routing.Common
{
    /// <summary>
    /// Default enpoint data source
    /// </summary>
    public abstract class EndpointDataSource
    {
        public abstract List<RouteEndpoint> Endpoints { get; }
    }
}

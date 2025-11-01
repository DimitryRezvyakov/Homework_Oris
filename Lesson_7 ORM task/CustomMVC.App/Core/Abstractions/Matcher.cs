using CustomMVC.App.Core.Http;
using CustomMVC.App.Core.Routing;
using CustomMVC.App.Core.Routing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Core.Abstractions
{
    public abstract class Matcher
    {
        protected readonly List<EndpointDataSource> _sources;
        public abstract Task<RouteEndpoint> MatchAsync(HttpContext context);

        public Matcher(List<EndpointDataSource> sources) { _sources = sources; }
    }
}

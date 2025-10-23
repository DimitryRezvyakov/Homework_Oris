using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniHttpServer.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPost : Attribute
    {
        public string Route { get; }
        public HttpPost(string route = null) 
        {
            Route = route;
        }
    }
}

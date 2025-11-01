using CustomMVC.App.Core.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Core.Http
{
    public class HttpContext
    {
        /// <summary>
        /// Virtual only for test purpose
        /// </summary>
 
        public virtual HttpRequest Request { get; set; }
        public virtual HttpResponse Response { get; }
        public virtual RouteEndpoint Endpoint { get; set; }
        public virtual IServiceProvider RequestServices { get; set; }
        public virtual Dictionary<string, string> RouteParametrs { get; set; }
        public virtual Dictionary<object, object> Items { get; } = new Dictionary<object, object>();

        public HttpContext(HttpRequest request, HttpResponse response) 
        {
            Request = request;
            Response = response;
        }

        /// <summary>
        /// For test purpose only
        /// </summary>
        public HttpContext()
        {

        }
    }
}

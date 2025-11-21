using CustomMVC.App.Core.Http.HttpMethods.Attributes;
using CustomMVC.App.Core.Middleware;
using CustomMVC.App.Core.Routing;
using CustomMVC.App.Core.Routing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Hosting.Application.Extensions
{
    public static class WebApplicationMapMethods
    {
        /// <summary>
        /// Adds a new MapGet route
        /// </summary>
        /// <param name="pattern">Route pattern</param>
        /// <param name="handler">Route handler</param>
        public static void MapGet(this WebApplication app, string pattern, RequestDelegate handler)
        {
            var enpointBuilder = new RouteEndpointBuilder(pattern, handler, 1);

            enpointBuilder.Metadata.Add(new HttpGet());

            var enpointDataSource = app.WebAppBuilder.Sources.OfType<DefaultEndpointDataSource>().First();

            if (enpointDataSource != null)
                enpointDataSource.Add(enpointBuilder);
        }

        /// <summary>
        /// Adds a new MapPost route
        /// </summary>
        /// <param name="pattern">Route pattern</param>
        /// <param name="handler">Route handler</param>
        public static void MapPost(this WebApplication app, string pattern, RequestDelegate handler)
        {
            var enpointBuilder = new RouteEndpointBuilder(pattern, handler, 1);

            enpointBuilder.Metadata.Add(new HttpPost());

            var enpointDataSource = app.WebAppBuilder.Sources.OfType<DefaultEndpointDataSource>().First();

            if (enpointDataSource != null)
                enpointDataSource.Add(enpointBuilder);
        }
    }
}

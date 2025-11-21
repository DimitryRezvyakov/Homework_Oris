using CustomMVC.App.Core.Routing.Common;
using CustomMVC.App.Core.Routing.Extensions;
using CustomMVC.App.Hosting.Application;
using CustomMVC.App.MVC.Controllers.Common;
using CustomMVC.App.MVC.Controllers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Extensions
{
    public static class MVCWebApplicationExtensions
    {
        public static void UseControllersWithViews(this WebApplication app)
        {
            app.UseRouting();

            app.WebAppBuilder.AddEndpointDataSource(ControllerEndpointDataSource.Instance);

            app.UseEndpoints();
        }

        /// <summary>
        /// Creating a new endpoint
        /// </summary>
        /// <param name="name">Endpoint name</param>
        /// <param name="pattern">Endpoint pattern</param>
        /// <param name="defaults">Endpoint defaults</param>
        public static void MapControllerRoute(this WebApplication app, string name, string pattern, Defaults? defaults = null)
        {
            var controllerRouteBulder = new ControllerRouteEnpointBuilder(name, pattern, defaults);

            var controllerEndpointDataSource = app.WebAppBuilder.Sources.OfType<ControllerEndpointDataSource>().First();

            if (controllerEndpointDataSource != null)
            {
                controllerEndpointDataSource.Add(controllerRouteBulder);
            }
        }
    }
}

using CustomMVC.App.Core.Http.HttpMethods.Attributes;
using CustomMVC.App.Core.Middleware;
using CustomMVC.App.Core.Routing;
using CustomMVC.App.Core.Routing.Common;
using CustomMVC.App.DependencyInjection;
using CustomMVC.App.MVC.Controllers.Abstractions;
using CustomMVC.App.MVC.Controllers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Routing
{
    /// <summary>
    /// Builds the endpoint
    /// </summary>
    public class ControllerRouteEnpointBuilder : EndpointBuilder
    {
        private static readonly ServiceCollection _services = ServiceCollection.Instance;

        /// <summary>
        /// Provides the action descriptors
        /// </summary>
        private readonly IActionDescriptorProvider actionDescriptorProvider = _services.GetService<IActionDescriptorProvider>();

        /// <summary>
        /// Provides the Controllers
        /// </summary>
        private readonly IControllersProvider controllersProvider = _services.GetService<IControllersProvider>();

        /// <summary>
        /// Route name
        /// </summary>
        private string _name;

        /// <summary>
        /// Route pattern
        /// </summary>
        private string _pattern;

        /// <summary>
        /// Route defaults
        /// </summary>
        private Defaults? _defaults;

        /// <summary>
        /// Route order
        /// </summary>
        private int _order;

        /// <summary>
        /// For MVC handler is the chain of responsability
        /// </summary>
        private RequestDelegate _handler = MVCRequestDelegateFactory.Create;

        public ControllerRouteEnpointBuilder(string pattern, Defaults? defaults = null)
        {
            _name = "default";
            _pattern = pattern;
            _defaults = defaults;
            _order = 1;
        }

        public ControllerRouteEnpointBuilder(string name, string pattern, Defaults? defaults = null)
        {
            _name = name;
            _pattern = pattern;
            _defaults = defaults;
            _order = 1;
        }

        public RequestDelegate? Handler { get => _handler;}

        public override RouteEndpoint Build()
        {
            List<object> EndpointMetadata = new();
            List<ActionDescriptor> RouteDescriptors = new();

            var routePattern = new RoutePattern(_name, _pattern, _defaults);

            //Controller must be in the pattern
            string controllerName = routePattern.Defaults?.ControllerName ?? routePattern.RouteTemplate.ControllerName!;
            string? actionName = routePattern.Defaults?.ActionName ?? routePattern.RouteTemplate.ActionName;

            var controller = controllersProvider.GetController(controllerName);

            EndpointMetadata.Add(controller!);

            //if action is null, all actions for controller will be included
            if (actionName == null)
            {
                foreach (var action in controller.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsSpecialName)
            .ToList())
                {
                    RouteDescriptors.Add(actionDescriptorProvider.GetDescriptor(action.Name));
                }
            }
            else
            {
                var descriptor = actionDescriptorProvider.GetDescriptor(actionName);
                var actionMethod = descriptor.HttpMethods.First();
                EndpointMetadata.Add(actionMethod ?? new HttpGet());
                RouteDescriptors.Add(descriptor);
            }

            EndpointMetadata.Add((IReadOnlyList<ActionDescriptor>)RouteDescriptors);

            return new RouteEndpoint(routePattern, _handler, 1, new RouteEndpointMetadata(EndpointMetadata));
        }
    }
}

using CustomMVC.App.Core.Http.HttpMethods.Abstractions;
using CustomMVC.App.DependencyInjection;
using CustomMVC.App.MVC.Controllers.Abstractions;
using CustomMVC.App.MVC.Controllers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Common
{
    /// <summary>
    /// Provides all descriptor for all controllers actions
    /// </summary>
    public class ActionDescriptorProvider : IActionDescriptorProvider
    {
        private Dictionary<string, ActionDescriptor> _descriptors { get; } = new();

        public ActionDescriptor GetDescriptor(string name)
        {
            return _descriptors[name];
        }

        public ActionDescriptorProvider(IControllersProvider controllersProvider)
        {
            var controllers = controllersProvider.GetControllersTypes();

            foreach (var controllerType in controllers)
            {
                foreach (var method in controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                                        .Where(m => !m.IsSpecialName))
                {
                    _descriptors.Add(method.Name, new ActionDescriptor(method));
                }
            }
        }
    }
}

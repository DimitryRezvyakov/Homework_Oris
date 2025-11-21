using CustomMVC.App.Common.Extensions;
using CustomMVC.App.MVC.Controllers.Abstractions;
using CustomMVC.App.MVC.Controllers.Common.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Common
{
    public class ControllersProvider : IControllersProvider
    {
        private ConcurrentDictionary<string, Type> _controllers { get; } = new();

        public Type GetController(string name)
        {
            return _controllers[name.ToLower()];
        }

        public string[] GetControllersNames()
        {
            return _controllers.Keys.ToArray();
        }

        public Type[] GetControllersTypes()
        {
            return _controllers.Values.ToArray();
        }

        public ControllersProvider()
        {
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();

            var controllers = assembly.GetTypes().Where(t => typeof(ControllerBase).IsAssignableFrom(t) && !t.IsAbstract).ToList();

            foreach (var controller in controllers)
            {
                string name = controller.Name.RawControllerName();

                _controllers.TryAdd(name, controller);
            }
        }
    }
}

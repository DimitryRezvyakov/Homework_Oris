using CustomMVC.App.MVC.Controllers.Abstractions;
using CustomMVC.App.MVC.Controllers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFrameworkTests.Samples.Mock
{
    public class ActionDescriptorProviderMock : IActionDescriptorProvider
    {
        private readonly Dictionary<string, ActionDescriptor> _descriptors = new()
        {
            { "Index", new ActionDescriptor(typeof(HomeController).GetMethod(nameof(HomeController.Index))!) },
            { "PostRequest", new ActionDescriptor(typeof(HomeController).GetMethod(nameof(HomeController.PostRequest))!) }
        };

        public ActionDescriptor GetDescriptor(string name)
        {
            return _descriptors[name];
        }
    }
}

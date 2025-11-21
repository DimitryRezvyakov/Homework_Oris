using CustomMVC.App.MVC.Controllers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Abstractions
{
    public interface IActionDescriptorProvider
    {
        private Dictionary<string, ActionDescriptor> _descriptors { get => _descriptors; }
        public ActionDescriptor GetDescriptor(string name);
    }
}

using CustomMVC.App.Core.Http;
using CustomMVC.App.MVC.Controllers.Common.Entities;
using CustomMVC.App.MVC.Controllers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Abstractions
{
    /// <summary>
    /// Abstract class for model binders
    /// </summary>
    public interface IModelBinder
    {
        public ModelState Bind(HttpContext context, ActionDescriptor descriptor);
        public bool CanBind(HttpContext context, ActionDescriptor descriptor);
    }
}

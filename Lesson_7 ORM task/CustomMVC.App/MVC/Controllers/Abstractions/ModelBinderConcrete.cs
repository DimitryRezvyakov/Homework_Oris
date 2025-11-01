using CustomMVC.App.Core.Http;
using CustomMVC.App.MVC.Controllers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Abstractions
{
    /// <summary>
    /// Abstract class for concrete model binders
    /// </summary>
    public abstract class ModelBinderConcrete
    {
        public abstract object? Bind(HttpContext context, ParameterDescriptor parameter);

        public abstract bool CanBind(HttpContext context, ParameterDescriptor parameter);
    }
}

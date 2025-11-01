using CustomMVC.App.Core.Http;
using CustomMVC.App.MVC.Controllers.Abstractions;
using CustomMVC.App.MVC.Controllers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Common.ModelBinding.Binders
{
    public class FromBodyBinder : ModelBinderConcrete
    {
        private readonly FromJsonBinder _jsonBinder = new FromJsonBinder();
        private readonly FromFormBinder _formBinder = new FromFormBinder();

        public override bool CanBind(HttpContext context, ParameterDescriptor parameter)
        {
            if (context.Request.ContentType?.Contains("application/json", StringComparison.OrdinalIgnoreCase) == true)
                return _jsonBinder.CanBind(context, parameter);

            if (context.Request.ContentType?.Contains("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase) == true)
                return _formBinder.CanBind(context, parameter);

            return false;
        }

        public override object? Bind(HttpContext context, ParameterDescriptor parameter)
        {
            if (context.Request.ContentType?.Contains("application/json", StringComparison.OrdinalIgnoreCase) == true)
                return _jsonBinder.Bind(context, parameter);

            if (context.Request.ContentType?.Contains("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase) == true)
                return _formBinder.Bind(context, parameter);

            return parameter.ParameterType.IsValueType
                ? Activator.CreateInstance(parameter.ParameterType)
                : null;
        }
    }
}

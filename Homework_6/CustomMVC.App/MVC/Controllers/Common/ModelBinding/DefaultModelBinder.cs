using CustomMVC.App.Core.Http;
using CustomMVC.App.DependencyInjection;
using CustomMVC.App.MVC.Controllers.Abstractions;
using CustomMVC.App.MVC.Controllers.Common.Entities;
using CustomMVC.App.MVC.Controllers.Common.ModelBinding.Attributes;
using CustomMVC.App.MVC.Controllers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Common.ModelBinding
{
    public class DefaultModelBinder : IModelBinder
    {
        /// <summary>
        /// Model binder factory to creating a ModelBinderConcrete instances
        /// </summary>
        private readonly IModelBinderFactory _factory;

        /// <summary>
        /// For DI and testing purpose only
        /// </summary>
        public DefaultModelBinder(IModelBinderFactory factory)
        {
            _factory = factory;
        }

        public ModelState Bind(HttpContext context, ActionDescriptor descriptor)
        {

            var modelState = new ModelState()
            {
                IsValid = true,
                Parameters = new object[0]
            };

            List<object> parameters = new List<object>();

            if (!descriptor.Parameters.Any())
            {
                return ModelState.FromSuccess();
            }

            foreach (var p in descriptor.Parameters)
            {
                var binder = _factory.Create(p?.BindingInfo?.ModelBinder ?? (new FromBody()).ModelBinderType);

                try
                {
                    var param = binder.Bind(context, p);

                    if (param == null || param == default)
                    {
                        parameters.Add(p.DefaultValue!);
                        modelState.IsValid = false;
                    }

                    else
                        parameters.Add(param);
                }
                catch (Exception)
                {
                    modelState.IsValid = false;
                    parameters.Add(p?.DefaultValue!);
                }
            }

            modelState.Parameters = parameters.ToArray();
            return modelState;
        }

        public bool CanBind(HttpContext context, ActionDescriptor descriptor)
        {
            if (!descriptor.Parameters.Any())
                return true;

            foreach (var p in descriptor.Parameters)
            {
                var binder = _factory.Create(p?.BindingInfo?.ModelBinder ?? (new FromBody()).ModelBinderType);

                if (!binder.CanBind(context, p))
                    return false;
            }
            return true;
        }
    }
}

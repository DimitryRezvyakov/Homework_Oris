using CustomMVC.App.MVC.Controllers.Abstractions;
using CustomMVC.App.MVC.Controllers.Common.ModelBinding.Attributes;
using CustomMVC.App.MVC.Controllers.Common.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Routing
{
    public class BindingInfo
    {
        /// <summary>
        /// Provides the source of binding
        /// </summary>
        public Type AttributeBindType { get; set; }

        /// <summary>
        /// Model binder type for binding source
        /// </summary>
        public Type ModelBinder { get; set; }


        public BindingInfo(ParameterInfo parameter)
        {
            ModelBindAttribute? attr = parameter.GetCustomAttributes(inherit: true)
                .OfType<ModelBindAttribute>()
                .FirstOrDefault();

            //By default binds from request body
            if (attr == null)
            {
                AttributeBindType = typeof(FromBody);
                ModelBinder = typeof(FromBodyBinder);
            }
            else
            {
                AttributeBindType = attr.GetType();
                ModelBinder = attr.ModelBinderType;
            }
        }
    }
}

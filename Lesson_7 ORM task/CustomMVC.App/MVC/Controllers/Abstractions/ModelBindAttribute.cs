using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Abstractions
{
    /// <summary>
    /// Abstract class for parameter model binding
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public abstract class ModelBindAttribute : Attribute, IModelBindType
    {
        public Type ModelBinderType { get; set; }
        public ModelBindAttribute(Type modelBindertype)
        {
            ModelBinderType = modelBindertype;
        }
    }
}

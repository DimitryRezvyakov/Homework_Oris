using CustomMVC.App.MVC.Controllers.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Common.ModelBinding.Attributes
{
    /// <summary>
    /// Support for custom binders
    /// </summary>
    /// <typeparam name="T">Custom binder type</typeparam>
    public class ModelBinder<T> : ModelBindAttribute where T : ModelBinderConcrete, new()
    {
        public ModelBinder() : base(typeof(T))
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Abstractions
{
    public interface IModelBinderFactory
    {
        public ModelBinderConcrete Create(Type type);
    }
}

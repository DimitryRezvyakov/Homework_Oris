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
    public class FromParamBinder : ModelBinderConcrete
    {
        public override object Bind(HttpContext context, ParameterDescriptor parameter)
        {
            throw new NotImplementedException();
        }

        public override bool CanBind(HttpContext context, ParameterDescriptor parameter)
        {
            throw new NotImplementedException();
        }
    }
}

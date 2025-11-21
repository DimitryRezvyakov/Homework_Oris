using CustomMVC.App.MVC.Controllers.Abstractions;
using CustomMVC.App.MVC.Controllers.Common.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Common.ModelBinding.Attributes
{
    /// <summary>
    /// Only for methadata, sets binding from body
    /// </summary>
    public class FromBody : ModelBindAttribute
    {
        public FromBody() : base(typeof(FromBodyBinder)) { }
    }
}

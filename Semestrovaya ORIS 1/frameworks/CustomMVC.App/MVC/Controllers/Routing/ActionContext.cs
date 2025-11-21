using CustomMVC.App.Core.Http;
using CustomMVC.App.MVC.Controllers.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Routing
{
    public class ActionContext
    {
        public Type ControllerType { get; set; }

        public ActionDescriptor ActionDescriptor { get; set; }

        public HttpContext Context { get; set; }

        public ModelState ModelState { get; set; }
    }
}

using CustomMVC.App.Core.Http;
using CustomMVC.App.MVC.Controllers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Abstractions
{
    public interface IActionSelector
    {
        public Task<ActionDescriptor> SelectBestCandidate(HttpContext context);
    }
}

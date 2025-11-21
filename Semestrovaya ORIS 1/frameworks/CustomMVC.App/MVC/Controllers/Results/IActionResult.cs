using CustomMVC.App.MVC.Controllers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Results
{
    public interface IActionResult
    {
        public Task ExecuteResultAsync(ActionContext context);
    }
}

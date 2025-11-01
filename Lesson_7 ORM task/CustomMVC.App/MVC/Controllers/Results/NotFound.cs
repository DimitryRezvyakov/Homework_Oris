using CustomMVC.App.MVC.Controllers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Results
{
    public class NotFound : IActionResult
    {
        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.Context.Response;

            response.SetStatusCode(404);

            await Task.CompletedTask;
        }
    }
}

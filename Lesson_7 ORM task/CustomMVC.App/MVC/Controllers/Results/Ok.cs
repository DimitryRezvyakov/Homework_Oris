using CustomMVC.App.MVC.Controllers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Results
{
    public class Ok : IActionResult
    {
        public Ok() { }
        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.Context.Response;

            response.SetStatusCode(200);

            await Task.CompletedTask;
        }
    }

    public class Ok<T> : IActionResult
    {
        private T _data { get; set; }

        public Ok(T data)
        {
            _data = data;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.Context.Response;

            response.SetStatusCode(200);

            response.SetContentType("application/json");

            var json = JsonSerializer.Serialize<T>(_data);

            if (json == null)
            {
                return;
            }

            await response.WriteAsync(json);
        }
    }
}

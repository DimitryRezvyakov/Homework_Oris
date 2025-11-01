using CustomMVC.App.Core.Http;
using CustomMVC.App.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Core.Middleware
{
    /// <summary>
    /// Represents a default middleware methods
    /// </summary>
    public interface IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next);
    }
}

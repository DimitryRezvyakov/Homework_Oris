using CustomMVC.App.Core.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Hosting.Application.Extensions
{
    public static class WebApplicationUseMethodsExtensions
    {
        /// <summary>
        /// Adds new Use middleware to pipeline
        /// </summary>
        /// <param name="middleware">Middleware</param>
        public static void Use(this WebApplication app, Func<HttpContext, Func<Task>, Task> middleware)
        {
            ArgumentNullException.ThrowIfNull(app.WebAppBuilder);

            app.WebAppBuilder.PipeLine.Use(middleware);
        }

        /// <summary>
        /// Adds new Run middleware to pipeline
        /// </summary>
        /// <param name="terminalMiddleware">Middleware</param>
        public static void Run(this WebApplication app, Func<HttpContext, Task> terminalMiddleware)
        {
            app.WebAppBuilder.PipeLine.Run(terminalMiddleware);
        }
    }
}

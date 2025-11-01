using CustomMVC.App.Common;
using CustomMVC.App.Hosting.Application;
using CustomMVC.App.Hosting.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Core.Middleware.Extensions
{
    public static class UseDefaultExeptionExtension
    {
        public static void UseDefaultExceptionHandler(this WebApplication app)
        {
            Logger<WebApplication> logger = new Logger<WebApplication>();

            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    logger.LogFatal(ex.Message, ex);
                }
            });
        }
    }
}

using CustomMVC.App.Common;
using CustomMVC.App.Common.Extensions;
using CustomMVC.App.Hosting.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Core.Middleware.Extensions
{
    public static class UseStaticFilesExtension
    {
        private static StaticFilesOptions _options = new();
        private static  readonly Logger<IMiddleware> _logger = new();

        /// <summary>
        /// Adds a middleware to pipline which will return a static files and terminate the pipline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="opt"></param>
        public static void UseStaticFiles(this WebApplication app, Func<StaticFilesOptions, Task>? opt = null)
        {
            if (opt != null) opt(_options);

            app.WebAppBuilder.PipeLine.Use(async (context, next) =>
            {
                byte[]? file;

                string absolutePath = context.Request?.Uri?.AbsolutePath ?? "/";

                if (Path.HasExtension(absolutePath))
                {
                    _logger.LogInfo("Trying to return static file");

                    string fileName = Path.GetFileName(absolutePath);

                    try
                    {
                        var files = Directory.GetFiles(_options.Root, fileName, SearchOption.AllDirectories);

                        if (files != null && files.Length > 0)
                        {
                            file = File.ReadAllBytes(files.First());

                            context.Response.SetStatusCode(200);
                            context.Response.SetContentType(Path.GetExtension(fileName).GetContentTypeByFileExtension());

                            await context.Response.WriteBytesAsync(file);
                        }

                        else
                        {
                            _logger.LogInfo($"File wasn`t found {absolutePath}");

                            context.Response.SetStatusCode(404);
                        }
                    }
                    catch (Exception)
                    {
                        _logger.LogInfo($"File wasn`t found {absolutePath}");

                        context.Response.SetStatusCode(404);
                    }
                }
                else
                {
                    await next();
                }
            });
        }
    }

    /// <summary>
    /// Configuration for <see cref="UseStaticFilesExtension.UseStaticFiles(WebApplication, Func{StaticFilesOptions, Task})">
    /// </summary>
    public class StaticFilesOptions
    {
        /// <summary>
        /// Sets the root directory where will be searched all static files.
        /// </summary>
        public string Root { get; set; } = "wwwroot";
    }
}

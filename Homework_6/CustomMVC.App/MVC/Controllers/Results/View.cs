using CustomMVC.App.Common;
using CustomMVC.App.DependencyInjection;
using CustomMVC.App.MVC.Controllers.Routing;
using CustomMVC.App.MVC.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Results
{
    public class View : IActionResult
    {
        private static readonly ServiceCollection _services = ServiceCollection.Instance;

        private static readonly IHtmlTemplateRenderer _htmlTemplateRenderer = _services.GetService<IHtmlTemplateRenderer>();

        private static readonly Logger<View> _logger = new();

        private object? _model { get; }

        public View(object? model = null)
        {
            _model = model;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            string[] paths =
            {
                $"Views/{context.ActionDescriptor.ControllerTypeInfo.Name.Replace("Controller", "")}/{context.ActionDescriptor.ActionName}.html",
            };

            var response = context.Context.Response;

            foreach (string path in paths)
            {
                try
                {
                    var html = _htmlTemplateRenderer.RenderFromFile(path, _model ?? new object[] { });

                    if (string.IsNullOrEmpty(html)) 
                            throw new Exception("file not found or can`t render template");

                    response.SetStatusCode(200);
                    response.SetContentType("text/html");
                    await response.WriteAsync(html);
                }

                catch (Exception ex)
                {
                    _logger.LogError(ex);
                }
            }
        }
    }
}

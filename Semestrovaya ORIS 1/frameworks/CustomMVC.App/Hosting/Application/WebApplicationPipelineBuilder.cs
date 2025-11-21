using CustomMVC.App.Core.Http;
using CustomMVC.App.Core.Middleware;
using CustomMVC.App.Core.Routing;
using CustomMVC.App.Hosting.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace CustomMVC.App.Hosting.Application
{
    public class WebApplicationPipelineBuilder : IWebApplicationPipelineBuilder
    {
        /// <summary>
        /// Application middlewares lists
        /// </summary>
        private readonly List<Func<RequestDelegate, RequestDelegate>> _middlewares = new();

        /// <summary>
        /// An endpoint router, if using endpoint just getting a HttpContext Endpoint and executing
        /// </summary>
        public RequestDelegate EndpointHandler { get; set; } = async (context) =>
        {
            context.Response.SetStatusCode(200);

            await Task.CompletedTask;
        };

        /// <summary>
        /// For DI and testing purpose only
        /// </summary>
        public WebApplicationPipelineBuilder() { }

        /// <summary>
        /// Adds new Use middleware to pipeline
        /// </summary>
        /// <param name="middleware">Middleware</param>
        /// <returns>WebApplicationPipeLineBuilder</returns>
        public IWebApplicationPipelineBuilder Use(Func<HttpContext, Func<Task>, Task> middleware)
        {
            _middlewares.Add(next => async ctx =>
            {
                await middleware(ctx, () => next(ctx));
            });

            return this;
        }

        /// <summary>
        /// Adds new Run middleware to pipline
        /// </summary>
        /// <param name="terminalMidleware">Middleware</param>
        /// <returns>WebApplicationPipeLineBuilder</returns>
        public IWebApplicationPipelineBuilder Run(Func<HttpContext, Task> terminalMidleware)
        {
            _middlewares.Add(next => async ctx => await terminalMidleware(ctx));

            return this;
        }

        /// <summary>
        /// Builds a web application pipeline
        /// </summary>
        /// <returns></returns>
        public RequestDelegate Build()
        {
            //if no one middlewares returning 404 status code
            if (_middlewares.Count == 0)
                return ctx =>
                {
                    ctx.Response.SetStatusCode(404);
                    return Task.CompletedTask;
                };

            RequestDelegate component = EndpointHandler;

            for (int i = _middlewares.Count - 1; i >= 0; i--)
            {
                component = _middlewares[i](component);
            }

            return component;
        }
    }
}

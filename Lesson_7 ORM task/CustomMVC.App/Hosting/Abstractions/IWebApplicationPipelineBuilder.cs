using CustomMVC.App.Core.Http;
using CustomMVC.App.Core.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Hosting.Abstractions
{
    public interface IWebApplicationPipelineBuilder
    {
        public RequestDelegate EndpointHandler { get; set; }

        public IWebApplicationPipelineBuilder Use(Func<HttpContext, Func<Task>, Task> middleware);

        public IWebApplicationPipelineBuilder Run(Func<HttpContext, Task> terminalMiddleware);

        public RequestDelegate Build();
    }
}

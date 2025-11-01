using CustomMVC.App.Common;
using CustomMVC.App.Common.Abstractions;
using CustomMVC.App.Core.Http;
using CustomMVC.App.Core.Middleware;
using CustomMVC.App.Core.Routing;
using CustomMVC.App.Core.Routing.Common;
using CustomMVC.App.DependencyInjection;
using CustomMVC.App.Hosting.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Hosting.Application
{
    public class WebApplication
    {
        private readonly Logger<WebApplication> _logger = new();

        /// <summary>
        /// Service provider for injection
        /// </summary>
        private readonly ServiceCollection ServiceProvider = ServiceCollection.Instance;

        /// <summary>
        /// Builder for a WebApplication
        /// </summary>
        public readonly WebApplicationBuilder WebAppBuilder;

        /// <summary>
        /// WebApplication EndpointDataSource
        /// </summary>
        public readonly List<EndpointDataSource> endpointDataSources = new List<EndpointDataSource>();

        /// <summary>
        /// Host for WebApplication
        /// </summary>
        private readonly IHost _host;

        /// <summary>
        /// Handler for request
        /// </summary>
        private RequestDelegate? _requestDelegate;

        /// <summary>
        /// Creates a WebApplicationBuilder
        /// </summary>
        /// <returns>WebApplicationBuilder</returns>
        public static WebApplicationBuilder CreateBuilder(Action<WebApplicationBuilderOptions>? opt = null)
        {
            return new WebApplicationBuilder(opt);
        }

        public WebApplication(IHost host, WebApplicationBuilder builder)
        {
            WebAppBuilder = builder;
            _host = host;
        }

        /// <summary>
        /// Running the application
        /// </summary>
        /// <exception cref="ArgumentNullException">If application was not builded by WebApplicationBuilder</exception>
        public void Run()
        {
            ArgumentNullException.ThrowIfNull(WebAppBuilder);

            _requestDelegate = WebAppBuilder.PipeLine.Build();

            if (_requestDelegate == null)
                throw new ArgumentNullException("Request Delegate is null");

            if (_host is null)
                throw new ArgumentNullException("You must build the application using WebApplicationBuilder before running");

            _host.RequestDelegate = _requestDelegate;

            _host.Start();


            while(_host.isListening)
            {
                var command = Console.ReadLine();

                if (command != null & command == "/stop")
                    _host.Stop();
            }
        }

        /// <summary>
        /// Only for testing
        /// </summary>
        public void Stop()
        {
            _host.Stop();
        }
    }
}

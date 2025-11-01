using CustomMVC.App.Common;
using CustomMVC.App.Common.Abstractions;
using CustomMVC.App.Core.Routing;
using CustomMVC.App.Core.Routing.Common;
using CustomMVC.App.DependencyInjection;
using CustomMVC.App.Hosting.Abstractions;
using CustomMVC.App.Hosting.Host;
using CustomMVC.App.MVC.Controllers.Abstractions;
using CustomMVC.App.MVC.Controllers.Common;
using CustomMVC.App.MVC.Controllers.Common.ModelBinding;
using CustomMVC.App.MVC.Controllers.Routing;
using CustomMVC.App.MVC.Views;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Hosting.Application
{
    public class WebApplicationBuilder
    {
        public readonly ServiceProvider Services = ServiceProvider.GetInstance();

        /// <summary>
        /// A web application instance
        /// </summary>
        private WebApplication? _app {  get; set; }

        /// <summary>
        /// A host builder
        /// </summary>
        private IHostBuilder hostOptionsBuilder;

        /// <summary>
        /// A web application pipeline builder
        /// </summary>
        private readonly IWebApplicationPipelineBuilder pipelineBuilder;

        /// <summary>
        /// A web application endpoint data sources
        /// </summary>
        private List<EndpointDataSource> _endpointDataSources = new List<EndpointDataSource>() { DefaultEndpointDataSource.Instance };

        public IHostBuilder Host => hostOptionsBuilder;
        public List<EndpointDataSource> Sources => _endpointDataSources;
        public IWebApplicationPipelineBuilder PipeLine => pipelineBuilder;

        public WebApplicationBuilder(Action<WebApplicationBuilderOptions>? configurator)
        {
            var options = new WebApplicationBuilderOptions();

            if (configurator != null) configurator(options);

            foreach (var action in options.ServiceConfiguration.Values)
            {
                action(null);
            }

            hostOptionsBuilder = Services.GetService<IHostBuilder>();
            pipelineBuilder = Services.GetService<IWebApplicationPipelineBuilder>();
        }

        /// <summary>
        /// Builds a WebApplication
        /// </summary>
        /// <returns>WebApplication</returns>
        public WebApplication Build()
        {
            var app = new WebApplication(hostOptionsBuilder.Build(), this);

            _app = app;

            foreach (var endpointDataSource in _endpointDataSources)
                app.endpointDataSources.Add(endpointDataSource);

            return app;
        }

        /// <summary>
        /// Adds a new endpoint data source
        /// </summary>
        /// <param name="source"></param>
        public void AddEndpointDataSource(EndpointDataSource source)
        {
            //If web application already created we add source to web application sources
            if (_app != null)
                UpdateWebApplicationDataSources(source);

            //Instead adding it to builder
            _endpointDataSources.Add(source);
        }
        /// <summary>
        /// Add endpoint data source directly to web application sources
        /// </summary>
        /// <param name="source"></param>
        private void UpdateWebApplicationDataSources(EndpointDataSource source)
        {
            _app?.endpointDataSources.Add(source);
        }
    }

    public class WebApplicationBuilderOptions()
    {
        public static readonly ServiceProvider Services = ServiceProvider.GetInstance();

        public readonly Dictionary<Type, Action<object[]?>> ServiceConfiguration = new()
    {
        // Singleton services
        { typeof(IConfiguration), args => Services.AddSingleton<IConfiguration, Configuration>() },
        { typeof(IWebApplicationPipelineBuilder), args => Services.AddSingleton<IWebApplicationPipelineBuilder, WebApplicationPipelineBuilder>() },
        { typeof(IHostBuilder), args => Services.AddSingleton<IHostBuilder, HostBuilder>() },
        { typeof(IControllersProvider), args => Services.AddSingleton<IControllersProvider, ControllersProvider>() },
        { typeof(IActionDescriptorProvider), args => Services.AddSingleton<IActionDescriptorProvider, ActionDescriptorProvider>() },
        { typeof(IModelBinderFactory), args => Services.AddScoped<IModelBinderFactory, ModelBinderFactory>() },
        { typeof(IModelBinder), args => Services.AddSingleton<IModelBinder, DefaultModelBinder>() },
        { typeof(IHtmlTemplateRenderer), args => Services.AddSingleton<IHtmlTemplateRenderer, HtmlTemplateRenderer>() },
        
        // Scoped services
        { typeof(IActionInvokerFactory), args => Services.AddScoped<IActionInvokerFactory, ActionInvokerFactory>() },
        { typeof(IActionSelector), args => Services.AddScoped<IActionSelector, ActionSelector>() }
    };

        public void ConfigureControllersProvider<T>() where T : class, IControllersProvider
        {
            ServiceConfiguration[typeof(IControllersProvider)] = Services.AddSingleton<IControllersProvider, T>;
        }

        public void ConfigureActionDescriptorsProvider<T>() where T : class, IActionDescriptorProvider
        {
            ServiceConfiguration[typeof(IActionDescriptorProvider)] = Services.AddSingleton<IActionDescriptorProvider, T>;
        }
    }
}

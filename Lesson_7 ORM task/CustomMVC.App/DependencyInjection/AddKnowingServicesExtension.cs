using CustomMVC.App.Common;
using CustomMVC.App.Common.Abstractions;
using CustomMVC.App.Hosting.Abstractions;
using CustomMVC.App.Hosting.Application;
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
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
namespace CustomMVC.App.DependencyInjection
{
    public static class AddKnowingServicesExtension
    {
        public static void AddKnowingServices(this IServiceProviderCustom serviceProvider)
        {
            // Singleton
            serviceProvider.AddSingleton<IConfiguration, Configuration>();

            serviceProvider.AddSingleton<IWebApplicationPipelineBuilder, WebApplicationPipelineBuilder>();

            serviceProvider.AddSingleton<IHostBuilder, HostBuilder>();

            serviceProvider.AddSingleton<IControllersProvider, ControllersProvider>();

            serviceProvider.AddSingleton<IActionDescriptorProvider, ActionDescriptorProvider>();

            serviceProvider.AddScoped<IModelBinderFactory, ModelBinderFactory>();

            serviceProvider.AddSingleton<IModelBinder, DefaultModelBinder>();

            serviceProvider.AddSingleton<IHtmlTemplateRenderer, HtmlTemplateRenderer>();

            //Scoped
            serviceProvider.AddScoped<IActionInvokerFactory, ActionInvokerFactory>();

            serviceProvider.AddScoped<IActionSelector, ActionSelector>();

            //Transient
        }
    }
}

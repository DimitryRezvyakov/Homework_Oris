using CustomMVC.App.Common;
using CustomMVC.App.Common.Abstractions;
using CustomMVC.App.Core.Http;
using CustomMVC.App.Core.Middleware;
using CustomMVC.App.DependencyInjection;
using CustomMVC.App.MVC.Controllers.Abstractions;
using CustomMVC.App.MVC.Controllers.Common.Entities;
using CustomMVC.App.MVC.Controllers.Common.ModelBinding;
using CustomMVC.App.MVC.Controllers.Results;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Routing
{
    /// <summary>
    /// Creates a action invoker which will handle the endpoint
    /// </summary>
    public class ActionInvokerFactory : IActionInvokerFactory
    {
        private static readonly Logger<ActionInvokerFactory> _logger = new();

        /// <summary>
        /// Model binder to bind action parameters
        /// </summary>
        private readonly IModelBinder _binder;

        private readonly IServiceProviderCustom _serviceProvider;

        /// <summary>
        /// For DI and testing purpose only
        /// </summary>
        public ActionInvokerFactory(IModelBinder binder, IServiceProviderCustom serviceProvider)
        {
            _binder = binder;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Creates a RequestDelegate that will handle an endpoint
        /// </summary>
        /// <param name="context">HttpContext for this request</param>
        /// <param name="descriptor">Action descriptor for action</param>
        /// <returns></returns>
        public RequestDelegate Create(HttpContext context, ActionDescriptor descriptor)
        {
            //binding a action parameters from request data
            var model = _binder.Bind(context, descriptor);

            //Conext for the action
            ActionContext actionContext = new()
            {
                ModelState = model,
                ControllerType = descriptor.ControllerTypeInfo,
                ActionDescriptor = descriptor,
                Context = context,
            };

            //creating a controller instance
            var method = _serviceProvider.GetType().GetMethod(nameof(ServiceProvider.GetController));
            var controller = method?.Invoke(_serviceProvider, new object[] {descriptor.ControllerTypeInfo, null }) as ControllerBase;

            ArgumentNullException.ThrowIfNull(controller, $"Controller {descriptor.ControllerTypeInfo} was not defined");

            controller.Context = context;
            controller.ModelState = model;

            return async (context) => 
            {
                _logger.LogDebug($"Executing action {actionContext}");

                var result = descriptor.MethodInfo.Invoke(controller, model.Parameters);
                
                if (result is IActionResult executer)
                {
                    try
                    {
                        _logger.LogDebug($"Executing action result of action {actionContext}");

                        await executer.ExecuteResultAsync(actionContext);

                        return;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex);
                    }
                }

                _logger.LogWarning($"The action {actionContext} doesn`t return any IActionResult");
            };
        }
    }
}

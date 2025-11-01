using CustomMVC.App.Core.Http.HttpMethods.Abstractions;
using CustomMVC.App.Core.Http.HttpMethods.Attributes;
using CustomMVC.App.MVC.Controllers.Abstractions;
using CustomMVC.App.MVC.Controllers.Common.ModelBinding;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Routing
{
    public class ActionDescriptor
    {
        /// <summary>
        /// TypeInfo of Controller wherea action are declared
        /// </summary>
        public TypeInfo ControllerTypeInfo { get; set; } = null!;

        /// <summary>
        /// MethodInfo for action
        /// </summary>
        public MethodInfo MethodInfo { get; set; } = null!;

        /// <summary>
        /// Action name
        /// </summary>
        public string ActionName { get; set; } = null!;
        public string? AttributeRouteTemplate { get; set; }

        /// <summary>
        /// Http methods on which action will be invoked
        /// </summary>
        public IReadOnlyList<HttpMethodAttribute> HttpMethods { get; set; } = Array.Empty<HttpMethodAttribute>();

        /// <summary>
        /// Action parameters
        /// </summary>
        public List<ParameterDescriptor> Parameters { get; set; } = new();

        /// <summary>
        /// Action metedata
        /// </summary>
        public IList<object> ActionMetadata { get; set; } = new List<object>();

        public ActionDescriptor(MethodInfo methodInfo)
        {
            //Checking that`s the method belong to a controller class
            MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            ControllerTypeInfo = methodInfo.DeclaringType?.GetTypeInfo()
                ?? throw new InvalidOperationException("Method must belong to a controller class.");

            // Setting action name
            ActionName = methodInfo.Name;

            //Setting Action metadata from attributes
            ActionMetadata = methodInfo.GetCustomAttributes(inherit: true).ToList();

            //Getting HttpMethods attributes 
            var methods = methodInfo
                .GetCustomAttributes(inherit: true)
                .OfType<HttpMethodAttribute>()
                .ToList();

            //If zero, adding a default HttpGet
            if (methods.Count == 0)
                methods.Add(new HttpGet());

            HttpMethods = methods;

            //Setting method parameters info
            Parameters = methodInfo.GetParameters()
                .Select(p => new ParameterDescriptor(p))
                .ToList();
        }
    }
}

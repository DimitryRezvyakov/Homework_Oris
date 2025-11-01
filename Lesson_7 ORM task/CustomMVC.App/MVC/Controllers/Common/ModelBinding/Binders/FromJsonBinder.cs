using CustomMVC.App.Core.Http;
using CustomMVC.App.MVC.Controllers.Abstractions;
using CustomMVC.App.MVC.Controllers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Common.ModelBinding.Binders
{
    public class FromJsonBinder : ModelBinderConcrete
    {
        public override bool CanBind(HttpContext context, ParameterDescriptor parameter)
        {
            return context.Request.ContentLength > 0 &&
                   context.Request.ContentType?.Contains("application/json", StringComparison.OrdinalIgnoreCase) == true;
        }

        public override object? Bind(HttpContext context, ParameterDescriptor parameter)
        {
            try
            {
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                var body = reader.ReadToEndAsync().Result;

                if (string.IsNullOrWhiteSpace(body))
                    return GetDefault(parameter.ParameterType);

                var result = JsonSerializer.Deserialize(body, parameter.ParameterType, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result ?? GetDefault(parameter.ParameterType);
            }
            catch
            {
                return GetDefault(parameter.ParameterType);
            }
        }

        private static object? GetDefault(Type type)
            => type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}

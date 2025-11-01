using CustomMVC.App.Core.Http;
using CustomMVC.App.MVC.Controllers.Abstractions;
using CustomMVC.App.MVC.Controllers.Routing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CustomMVC.App.MVC.Controllers.Common.ModelBinding.Binders
{
    public class FromQueryBinder : ModelBinderConcrete
    {
        public override bool CanBind(HttpContext context, ParameterDescriptor parameter)
        {
            var query = ParseQuery(context.Request.Uri?.Query ?? "");

            if (IsSimple(parameter.ParameterType))
            {
                return query.TryGetValue(parameter.Name, out var value) && CanConvert(value, parameter.ParameterType);
            }

            foreach (var prop in parameter.ParameterType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (query.ContainsKey(prop.Name) && IsSimple(prop.PropertyType))
                    return true;
            }

            return false;
        }

        public override object? Bind(HttpContext context, ParameterDescriptor parameter)
        {
            var query = ParseQuery(context.Request.Uri?.Query ?? "");

            if (IsSimple(parameter.ParameterType))
            {
                query.TryGetValue(parameter.Name, out var value);
                return ConvertValue(value, parameter.ParameterType);
            }

            var instance = Activator.CreateInstance(parameter.ParameterType)!;

            foreach (var prop in parameter.ParameterType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!IsSimple(prop.PropertyType))
                    continue;

                if (query.TryGetValue(prop.Name, out var value))
                {
                    var converted = ConvertValue(value, prop.PropertyType);
                    prop.SetValue(instance, converted);
                }
            }

            return instance;
        }

        private static Dictionary<string, string> ParseQuery(string query)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(query))
                return result;

            if (query.StartsWith("?"))
                query = query.Substring(1);

            foreach (var pair in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
            {
                var kv = pair.Split('=', 2);
                if (kv.Length == 2)
                    result[HttpUtility.UrlDecode(kv[0])] = HttpUtility.UrlDecode(kv[1]);
            }

            return result;
        }

        private static bool IsSimple(Type type)
        {
            return type.IsPrimitive
                   || type == typeof(string)
                   || type == typeof(decimal)
                   || type == typeof(DateTime)
                   || type.IsEnum;
        }

        private static bool CanConvert(string value, Type type)
        {
            try
            {
                if (type.IsEnum)
                    Enum.Parse(type, value, ignoreCase: true);
                else
                    Convert.ChangeType(value, type);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static object? ConvertValue(string? value, Type type)
        {
            try
            {
                if (value == null)
                    return type.IsValueType ? Activator.CreateInstance(type) : null;

                if (type == typeof(string))
                    return value;

                if (type.IsEnum)
                    return Enum.Parse(type, value, true);

                return Convert.ChangeType(value, type);
            }
            catch
            {
                return type.IsValueType ? Activator.CreateInstance(type) : null;
            }
        }
    }
}
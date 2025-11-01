using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Common.ModelBinding.Binders
{
    using CustomMVC.App.Core.Http;
    using CustomMVC.App.MVC.Controllers.Abstractions;
    using CustomMVC.App.MVC.Controllers.Routing;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web;

    public class FromFormBinder : ModelBinderConcrete
    {
        public override bool CanBind(HttpContext context, ParameterDescriptor parameter)
        {
            // Поддерживаем только формы
            return context.Request.ContentType == "application/x-www-form-urlencoded";
        }

        public override object? Bind(HttpContext context, ParameterDescriptor parameter)
        {
            var form = context.Request.ContentType == "application/x-www-form-urlencoded"
                ? ParseForm(context)
                : new Dictionary<string, string>();

            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var kv in form)
                dict[kv.Key] = kv.Value.ToString();

            return BindValue(dict, parameter.ParameterType, prefix: null);
        }

        private object? BindValue(Dictionary<string, string> form, Type type, string? prefix)
        {
            // Простой тип — просто ищем значение по ключу
            if (IsSimple(type))
            {
                if (prefix != null && form.TryGetValue(prefix, out var valueStr))
                    return ConvertValue(valueStr, type);

                return GetDefault(type);
            }

            // Создаём экземпляр сложного типа
            var instance = Activator.CreateInstance(type)!;

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var key = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}.{prop.Name}";

                // Если есть точечные поля вроде Address.City — ищем их
                var hasDirectMatch = form.ContainsKey(key);
                var hasNestedMatch = false;

                // Если есть хоть одно поле с таким префиксом (например, Address.City)
                foreach (var k in form.Keys)
                {
                    if (k.StartsWith(key + ".", StringComparison.OrdinalIgnoreCase))
                    {
                        hasNestedMatch = true;
                        break;
                    }
                }

                if (hasDirectMatch || hasNestedMatch || IsSimple(prop.PropertyType))
                {
                    var propValue = BindValue(form, prop.PropertyType, key);
                    prop.SetValue(instance, propValue);
                }
            }

            return instance;
        }

        private static Dictionary<string, string> ParseForm(HttpContext context)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            var body = reader.ReadToEnd();

            if (string.IsNullOrWhiteSpace(body))
                return result;

            var pairs = body.Split('&', StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in pairs)
            {
                var kv = pair.Split('=', 2);
                if (kv.Length == 2)
                {
                    var key = HttpUtility.UrlDecode(kv[0]);
                    var value = HttpUtility.UrlDecode(kv[1]);
                    result[key] = value;
                }
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

        private static object? ConvertValue(string? value, Type type)
        {
            if (value == null)
                return GetDefault(type);

            try
            {
                if (type == typeof(string))
                    return value;

                if (type.IsEnum)
                    return Enum.Parse(type, value, true);

                return Convert.ChangeType(value, type);
            }
            catch
            {
                return GetDefault(type);
            }
        }

        private static object? GetDefault(Type type)
            => type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}

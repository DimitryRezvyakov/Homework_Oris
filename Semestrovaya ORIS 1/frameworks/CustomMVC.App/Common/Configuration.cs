using CustomMVC.App.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomMVC.App.Common
{
    public class Configuration : IConfiguration
    {
        private readonly Dictionary<string, object> _settings = new();
        private string _settingsFilePath = "settings.json";
        public Configuration()
        {
            var basePath = GetEntryAssemblyDirectory();
            var filePath = Path.Combine(basePath, _settingsFilePath);

            if (!File.Exists(filePath))
                return;

            var json = File.ReadAllText(filePath);
            var document = JsonDocument.Parse(json);
            _settings = ParseElement(document.RootElement);
        }

        public void SetSettingsFilePath(string path)
        {
            _settingsFilePath = path;
        }

        private static string GetEntryAssemblyDirectory()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null)
                return AppContext.BaseDirectory;

            return Path.GetDirectoryName(entryAssembly.Location)!;
        }

        private static Dictionary<string, object> ParseElement(JsonElement element)
        {
            var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            foreach (var property in element.EnumerateObject())
            {
                switch (property.Value.ValueKind)
                {
                    case JsonValueKind.Object:
                        dict[property.Name] = ParseElement(property.Value);
                        break;
                    case JsonValueKind.Array:
                        var list = new List<object>();
                        foreach (var item in property.Value.EnumerateArray())
                        {
                            if (item.ValueKind == JsonValueKind.Object)
                                list.Add(ParseElement(item));
                            else
                                list.Add(GetPrimitiveValue(item));
                        }
                        dict[property.Name] = list;
                        break;
                    default:
                        dict[property.Name] = GetPrimitiveValue(property.Value);
                        break;
                }
            }

            return dict;
        }

        private static object GetPrimitiveValue(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => element.TryGetInt64(out var i) ? i : element.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => element.GetRawText()
            };
        }

        public object? Get(params string[] keys)
        {
            object? current = _settings;

            foreach (var key in keys)
            {
                if (current is Dictionary<string, object> dict && dict.TryGetValue(key, out var next))
                {
                    current = next;
                }
                else
                {
                    throw new Exception();
                }
            }

            return current;
        }

        public T? Get<T>(params string[] keys)
        {
            var value = Get(keys);
            if (value == null) return default;

            if (value is JsonElement jsonElement)
            {
                return JsonSerializer.Deserialize<T>(jsonElement.GetRawText());
            }

            if (value is T tValue)
                return tValue;

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return default;
            }
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}

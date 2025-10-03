using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MiniHttpServer.Settings
{
    public class SettingsModel
    {

        public bool MultiplePrefixes { get; init; } = false;
        public string[]? Prefixes { get; init; }
        public string? ConnectionString { get; init; }

        public static JsonNode? GetOptions()
        {
            try
            {
                return JsonNode.Parse(File.ReadAllText("settings.json"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}

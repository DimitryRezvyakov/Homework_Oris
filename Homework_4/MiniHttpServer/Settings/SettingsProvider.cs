using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniHttpServer.Settings
{
    public class SettingsProvider
    {
        private static SettingsModel? instance;

        public static SettingsModel Settings
        {
            get
            {
                if (instance == null) instance = GetOptions();

                return instance;
            }
        }

        private static SettingsModel GetOptions()
        {
            try
            {
                var res =  JsonSerializer.Deserialize<SettingsModel>(File.ReadAllText("settings.json"));

                if (res is null) throw new Exception("Конфигурация не найдена");

                res.Prefix = $"http://{res.Domain}:{res.Port}/";

                return res;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Конфигурация не найдена");
            }
        }
    }
}

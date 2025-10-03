using System.Net;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using MiniHttpServer.Servers;
using MiniHttpServer.Settings;

namespace MiniHttpServer;

public class Program
{
    public static void Main(string[] args)
    {
        var settingsValues = SettingsModel.GetOptions();

        if (settingsValues == null) return;

        var finderConnectionString = $"http://{settingsValues["Domain"]}:{settingsValues["Port"]}/{settingsValues["FindUri"]}/";
        var chatgptConnectionString = $"http://{settingsValues["Domain"]}:{settingsValues["Port"]}/{settingsValues["ChatGPTUri"]}/";

        var server = new HttpServer(
            new SettingsModel()
            {
                MultiplePrefixes = true,
                Prefixes = new string[] { finderConnectionString, chatgptConnectionString }
            });

        server.Command = async (context) =>
        {
            var response = context.Response;
            string? responseText = null;

            if (context.Request.Url.ToString().EndsWith("Finder"))
                responseText = GetResponseText((string)settingsValues["FinderPath"]);

            else if (context.Request.Url.ToString().EndsWith("ChatGPT"))
                responseText = GetResponseText((string)settingsValues["ChatGPTPath"]);

            if (responseText == null)
            {
                responseText = "Ошибка сервера. Страница не найдена";
            }

            byte[] buffer = Encoding.UTF8.GetBytes(responseText);

            response.ContentLength64 = buffer.Length;
            using Stream output = response.OutputStream;

            await output.WriteAsync(buffer);
            await output.FlushAsync();
        };

        Task.Run(() =>
        {
            server.Start();
        });

        while (true)
        {
            if (Console.ReadLine() == "/stop")
            {
                server.cts.Cancel();
                server.Stop();
                break;
            }

        }
    }

    public static string? GetResponseText(string path)
    {
        try
        {
            return File.ReadAllText(path);
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine("Директория Public не найдена");
            return null;
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Файл не найден");
            return null;
        }
        catch (Exception)
        {
            Console.WriteLine("Ошибка при извлечении текста");
            return null;
        }
    }
}

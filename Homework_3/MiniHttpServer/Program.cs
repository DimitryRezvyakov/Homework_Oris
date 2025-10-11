using System.Net;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using MiniHttpServer.Servers;
using MiniHttpServer.Settings;
using MiniHttpServer.Shared;

namespace MiniHttpServer;

public class Program
{
    public static void Main(string[] args)
    {
        var settingsValues = SettingsProvider.Settings;

        var server = new HttpServer();

        server.Command = async (context) =>
        {
            var response = context.Response;

            string? path = context.Request.Url?.AbsolutePath;

            byte[]? responseBytes;

            if (path == null || path == "/")
                responseBytes = GetResponseBytes.Invoke($"Public/index.html");

            else
            {
                responseBytes = GetResponseBytes.Invoke($"{path}/");
            }

            if (responseBytes == null)
            {
                response.StatusCode = 404;
                response.OutputStream.Close();
            }

            else
            {
                response.ContentType = GetContentType.Invoke(path);
                response.ContentLength64 = responseBytes.Length;

                using Stream output = response.OutputStream;

                await output.WriteAsync(responseBytes);
                await output.FlushAsync();
            }
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
}

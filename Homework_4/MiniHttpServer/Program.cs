using System.Net;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using MiniHttpServer.Core.Handlers;
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

            var staticHandler = new StaticFileHandler();
            var endPointHandler = new EndpointHandler();

            staticHandler.Successor = endPointHandler;

            staticHandler.HandleRequest(context);
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

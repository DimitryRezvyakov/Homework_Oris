using CustomMVC.App.Common;
using CustomMVC.App.Common.Abstractions;
using CustomMVC.App.Core.Http;
using CustomMVC.App.Core.Middleware;
using CustomMVC.App.Hosting.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Hosting.Host
{
    public class HttpListenerHost : IHost
    {
        private readonly HttpListener _listener = new();
        private readonly HostOptions _options;
        public RequestDelegate RequestDelegate { get; set; } = null!;
        public bool isListening => _listener.IsListening;
        private readonly Logger<HttpListenerHost> _logger = new();

        public HttpListenerHost(HostOptions options) 
        {
            _options = options;
        }

        public async Task ListenAsync()
        {
            try
            {
                _logger.LogDebug("Awaiting for request");

                var context = await _listener.GetContextAsync();

                _ = HandleRequest(context);
            }
            catch (ObjectDisposedException)
            {
                _logger.LogInfo("The server was stopped");
            }

            await ListenAsync();
        }

        public async Task HandleRequest(HttpListenerContext context)
        {
            var httpContext = new HttpContext(new HttpRequest(context.Request), new HttpResponse(context.Response));

            try
            {
                await RequestDelegate.Invoke(httpContext);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }
            finally
            {
                _logger.LogDebug("Sending response");

                context.Response.Close();
            }
        }

        public void Shutdown()
        {
            _listener.Abort();
        }

        public void Start()
        {
            _listener.Prefixes.Add(_options.ConnectionString);

            _listener.Start();

            _logger.LogInfo($"Now listening on {_options.ConnectionString}");

            _ = ListenAsync();
        }

        public void Stop()
        {
            _listener.Stop();
        }
    }
}

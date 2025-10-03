using MiniHttpServer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MiniHttpServer.Servers
{
    public class HttpServer
    {

        private HttpListener _listener;
        public SettingsModel _settings { get; init; }


        public CancellationTokenSource cts = new CancellationTokenSource();
        public bool isRunning => _listener.IsListening;
        public Action<HttpListenerContext>? Command { get; set; }

        public HttpServer(SettingsModel settingsModel)
        {
            _listener = new HttpListener();
            _settings = settingsModel;
        }

        public void Start()
        {
            if (_settings.MultiplePrefixes)
            {

                if (_settings.Prefixes == null || _settings.Prefixes.Length == 0)
                {
                    Console.WriteLine("Отсутстувую строки для подключения");
                    return;
                }

                foreach (var s in _settings.Prefixes)
                {
                    _listener.Prefixes.Add(s);
                }
            }

            else
            {
                if (string.IsNullOrEmpty(_settings.ConnectionString))
                {
                    Console.WriteLine("Строка для подключения не найдена");
                    return;
                }

                _listener.Prefixes.Add(_settings.ConnectionString);
            }

            try
            {
                _listener.Start();
                Receive();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void Stop()
        {
            if (!_listener.IsListening)
            {
                Console.WriteLine("Сервер еще не запущен");
            }

            _listener.Stop();
        }

        private void Receive()
        {
            var result = _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        }

        private void ListenerCallback(IAsyncResult ar)
        {
            if (_listener.IsListening)
            {
                if (cts.IsCancellationRequested) return;

                var context = _listener.EndGetContext(ar);

                if (Command != null)
                {
                    Command(context);
                }

                else
                {
                    var request = context.Request;

                    Console.WriteLine($"{request.Url}");
                }

                Receive();
            }
        }
    }
}

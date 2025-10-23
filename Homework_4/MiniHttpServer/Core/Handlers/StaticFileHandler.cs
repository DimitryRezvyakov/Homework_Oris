using MiniHttpServer.Core.Abstracts;
using MiniHttpServer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MiniHttpServer.Core.Handlers
{
    public class StaticFileHandler : Handler
    {

        public override void HandleRequest(HttpListenerContext context)
        {
            // некоторая обработка запроса

            bool isGetMethod = context.Request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase);
            bool isStaticFile = Path.HasExtension(context.Request.Url.AbsolutePath.Trim('/'));

            if (isGetMethod && isStaticFile)
            {
                var request = context.Request;
                var response = context.Response;

                string? path = context.Request.Url?.AbsolutePath;

                path = path?.Replace("%20", " ");

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

                    output.WriteAsync(responseBytes);
                    output.FlushAsync();
                }
            }

            else if (Successor != null)
            {
                Successor.HandleRequest(context);
            }
        }
    }
}

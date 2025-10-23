using MiniHttpServer.Core.Abstracts;
using MiniHttpServer.Core.Attributes;
using MiniHttpServer.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace MiniHttpServer.Core.Handlers
{
    public class EndpointHandler : Handler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            if (true)
            {
                var requset = context.Request;

                var endpointName = requset.Url?.AbsolutePath.Split("/").Where(p => p != string.Empty && p != null).First();

                var assembly = Assembly.GetExecutingAssembly();

                var endpoint = assembly.GetTypes()
                    .Where(t => t.GetCustomAttribute<Endpoint>() != null)
                    .FirstOrDefault(end => IsCheckedNameEndpoin(endpointName?.ToLower(), end.Name.ToLower()));

                if (endpoint == null)
                {
                    context.Response.StatusCode = 404;
                    context.Response.Close();
                }

                var method = endpoint?.GetMethods().Where(t => t.GetCustomAttributes(true)
                 .Any(attr => attr.GetType().Name.Equals($"Http{context.Request.HttpMethod}", StringComparison.OrdinalIgnoreCase)))
                 .FirstOrDefault();

                if (method == null) return;

                object? ret;

                if (context.Request.HttpMethod.Equals("Post", StringComparison.OrdinalIgnoreCase))
                {
                    var content = GetBodyString(context.Request.InputStream, context.Request.ContentEncoding).Split("&");

                    Dictionary<string, string> data = new();

                    foreach (var item in content)
                    {
                        data.TryAdd(item.Split("=")[0], item.Split("=")[1]);
                    }

                    data.TryGetValue("email", out var email);
                    data.TryGetValue("password", out var password);

                    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                    {
                        context.Response.StatusCode = 404;
                        context.Response.Close();

                        return;
                    }

                    ret = method.Invoke(Activator.CreateInstance(endpoint), new object[] {email, password});
                }

                else
                {
                    ret = method.Invoke(Activator.CreateInstance(endpoint), null);
                }

                if (ret is string responseFilePath)
                {
                    var response = context.Response;

                    var responseBytes = GetResponseBytes.Invoke(responseFilePath);

                    if (responseBytes == null)
                    {
                        response.StatusCode = 404;
                        response.Close();

                        return;
                    }

                    response.ContentType = GetContentType.Invoke(responseFilePath);
                    response.ContentLength64 = responseBytes.Length;

                    using Stream output = response.OutputStream;

                    output.WriteAsync(responseBytes);
                    output.FlushAsync();
                }
            }
        }

        public bool IsCheckedNameEndpoin(string? className, string endpointName) => 
            endpointName.Equals(className, StringComparison.OrdinalIgnoreCase) ||
            endpointName.Equals($"{className}Endpoint", StringComparison.OrdinalIgnoreCase);

        public static string GetBodyString(Stream stream, Encoding encoding)
        {
            using (var reader = new StreamReader(stream, encoding))
            {
                var bodyString = reader.ReadToEnd();

                return HttpUtility.UrlDecode(bodyString);
            }
        }
        public T? Bind<T>(string json)
        {
            try
            {
                var res = JsonSerializer.Deserialize<T>(json);

                if (res == null)
                    return default;
                return res;
            }
            catch (Exception ex)
            {
                return default;
            }
        }
    }
}

using CustomMVC.App.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Core.Http
{
    public class HttpResponse
    {
        private readonly HttpListenerResponse _inner;
        private bool _hasStarted = false;
        public virtual bool HasStarted => _hasStarted;
        public virtual Encoding? ContentEncoding => _inner.ContentEncoding;
        public virtual string? ContentType => _inner.ContentType;
        public virtual long ContentLength => _inner.ContentLength64;
        public virtual CookieCollection Cookies => _inner.Cookies;
        public virtual NameValueCollection Headers => _inner.Headers;
        public virtual bool KeepAlive => _inner.KeepAlive;
        public virtual Stream OutputStream => _inner.OutputStream;
        public virtual int StatusCode => _inner.StatusCode;
        public void Redirect(string url) => _inner.Redirect(url);

        public HttpResponse(HttpListenerResponse inner)
        {
            _inner = inner;
        }

        /// <summary>
        /// For testing purpose only
        /// </summary>
        public HttpResponse() { }

        public async Task WriteAsync(string text)
        {
            if (!_hasStarted)
            {
                _hasStarted = true;
            }

            var buffer = System.Text.Encoding.UTF8.GetBytes(text);

            this.SetContetnLength(buffer.LongLength);

            await _inner.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        }

        public async Task WriteBytesAsync(byte[] bytes)
        {
            if (!_hasStarted)
            {
                _hasStarted = true;
            }

            this.SetContetnLength(bytes.LongLength);

            await _inner.OutputStream.WriteAsync(bytes, 0, bytes.Length);
        }

        public void SetContentType(string contentType)
        {
            _inner.ContentType = contentType;
        }

        public void SetContetnLength(long length)
        {
            _inner.ContentLength64 = length;
        }

        public void SetHeader(string key, string value)
        {
            _inner.Headers.Add(key, value);
        }

        public void SetStatusCode(int statusCode)
        {
            _inner.StatusCode = statusCode;
        }

        public void Close()
        {
            var services = ServiceCollection.Instance;

            services.ClearScope();

            _inner.Close();
        }
    }
}

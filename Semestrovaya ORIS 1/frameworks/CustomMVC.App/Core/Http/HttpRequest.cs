using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Collections.Specialized;

namespace CustomMVC.App.Core.Http
{
    public class HttpRequest
    {
        private readonly HttpListenerRequest _inner;
        public virtual Uri? Uri => _inner.Url;
        public virtual string? ContentType => _inner.ContentType;
        public virtual long ContentLength => _inner.ContentLength64;
        public virtual Uri? UriReferer => _inner.UrlReferrer;
        public virtual string Method => _inner.HttpMethod;
        public virtual Stream Body => _inner.InputStream;
        public virtual NameValueCollection Headers => _inner.Headers;
        public virtual NameValueCollection QueryString => _inner.QueryString;
        public virtual string[]? Language => _inner.UserLanguages;
        public CookieCollection Cookie => _inner.Cookies;
        public HttpRequest(HttpListenerRequest inner)
        {
            _inner = inner;
        }

        /// <summary>
        /// Only for test purpose
        /// </summary>
        public HttpRequest() { }
    }
}

using CustomMVC.App.Core.Http.HttpMethods.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Core.Http.HttpMethods.Attributes
{
    public class HttpGet : HttpMethodAttribute
    {
        public HttpGet() : base("GET") { }
    }
}

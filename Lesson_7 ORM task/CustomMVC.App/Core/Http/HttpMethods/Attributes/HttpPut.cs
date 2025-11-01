using CustomMVC.App.Core.Http.HttpMethods.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Core.Http.HttpMethods.Attributes
{
    public class HttpPut : HttpMethodAttribute
    {
        public HttpPut() : base("PUT") { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Core.Http.HttpMethods.Abstractions
{
    public abstract class HttpMethodAttribute : Attribute, IHttpMethodMetadata
    {
        public IReadOnlyList<string> Methods { get; set; }

        public HttpMethodAttribute(params string[] args)
        {
            Methods = [.. args];
        }
    }
}

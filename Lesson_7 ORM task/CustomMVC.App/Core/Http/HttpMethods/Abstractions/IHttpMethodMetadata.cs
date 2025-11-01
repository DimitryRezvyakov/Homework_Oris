using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Core.Http.HttpMethods.Abstractions
{
    public interface IHttpMethodMetadata
    {
        IReadOnlyList<string> Methods { get; }
    }
}

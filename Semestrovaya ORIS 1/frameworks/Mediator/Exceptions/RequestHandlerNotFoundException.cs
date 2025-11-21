using Mediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Exceptions
{
    public class RequestHandlerNotFoundException : Exception
    {
        public override string Message { get; }
        public RequestHandlerNotFoundException(Type requestType) { Message = $"Can`t find handler for {requestType}"; }
    }
}

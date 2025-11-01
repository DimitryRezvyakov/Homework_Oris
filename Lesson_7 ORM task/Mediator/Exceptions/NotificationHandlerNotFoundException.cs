using Mediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Exceptions
{
    public class NotificationHandlerNotFoundException : Exception
    {
        public NotificationHandlerNotFoundException(Type notificationType) { Message = $"Can`t find handler for {notificationType}"; }

        public override string Message { get; }
    }
}

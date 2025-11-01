using CustomMVC.App.Core.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Core.Middleware
{
    public delegate Task RequestDelegate(HttpContext context);
}

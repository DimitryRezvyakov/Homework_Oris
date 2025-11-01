using CustomMVC.App.Core.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Hosting.Abstractions
{

    /// <summary>
    /// Default host interface
    /// </summary>
    public interface IHost
    {
        public bool isListening { get; }
        public RequestDelegate RequestDelegate { get; set; }
        public void Start();
        public void Shutdown();
        public void Stop();
    }
}

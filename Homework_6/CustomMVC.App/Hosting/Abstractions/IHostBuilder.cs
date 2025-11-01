using CustomMVC.App.Core.Abstractions;
using CustomMVC.App.Hosting.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Hosting.Abstractions
{
    /// <summary>
    /// Default host builder interface
    /// </summary>
    public interface IHostBuilder
    {
        public HttpListenerHost Build();

        public IHostBuilder Configure(Action<HostOptions> opt);
    }
}

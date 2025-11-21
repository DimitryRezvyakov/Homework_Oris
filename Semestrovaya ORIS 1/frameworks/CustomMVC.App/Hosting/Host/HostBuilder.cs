using CustomMVC.App.Hosting.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Hosting.Host
{
    public class HostBuilder : IHostBuilder
    {
        /// <summary>
        /// Host options
        /// </summary>
        private readonly HostOptions _options = new();

        /// <summary>
        /// For DI and testing purpose only
        /// </summary>
        public HostBuilder() { }

        /// <summary>
        /// Builds new http host
        /// </summary>
        /// <returns></returns>
        public HttpListenerHost Build()
        {
            return new HttpListenerHost(_options);
        }

        /// <summary>
        /// Configure the host options
        /// </summary>
        /// <param name="opt">Configure action</param>
        /// <returns>IHostBuilder</returns>
        public IHostBuilder Configure(Action<HostOptions>? opt)
        {
            opt?.Invoke(_options);
            
            return this;
        }
    }
}

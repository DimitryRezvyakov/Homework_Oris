using CustomMVC.App.Common.Abstractions;
using CustomMVC.App.DependencyInjection;
using CustomMVC.App.Hosting.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Hosting.Host
{
    public enum Environment
    {
        Development, Production
    }

    public class HostOptions
    {
        private readonly ServiceCollection _services = ServiceCollection.Instance;

        /// <summary>
        /// Trying to set settings from settings.json file
        /// </summary>
        public HostOptions()
        {
            try
            {
                var config = _services.GetService<IConfiguration>();

                var domain = config.Get("Host", "Domain") as string;
                var protocol = config.Get("Host", "Protocol") as string;
                var port = config.Get("Host", "Port") as string;

                var connectionString = $"{protocol}://{domain}:{port}/";

                Domain = domain!;
                Port = port!;
                ConnectionString = connectionString;
            }
            catch
            {

            }
        }

        /// <summary>
        /// Host domain
        /// </summary>
        public string Domain { get; set; } = "localhost";

        /// <summary>
        /// Host port
        /// </summary>
        public string Port { get; set; } = "8888";

        /// <summary>
        /// Host connection string
        /// </summary>
        public string ConnectionString { get; set; } = "http://localhost:8888/";

        /// <summary>
        /// Host name
        /// </summary>
        public string HostName { get; set; } = Assembly.GetCallingAssembly().ToString();

        /// <summary>
        /// Application name
        /// </summary>
        public string ApplicationName { get; set; } = "MyApp";

        /// <summary>
        /// Application environment
        /// </summary>
        public Environment ApplicationEnvironment { get; set; } = Environment.Development;
    }
}

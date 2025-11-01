using CustomMVC.App.Common.Abstractions;
using CustomMVC.App.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Common
{
    public enum LogLevel
    {
        Info,
        Debug,
    }

    public class Logger<T> : Ilogger<T>
    {
        private LogerOptions _options = new();
        public Type type => typeof(T);

        public void LogError(Exception ex)
        {
            Console.WriteLine($"Error: {ex} in {type}");
        }

        public void LogFatal(string message, Exception ex)
        {
            Console.WriteLine($"Fatal: {message} in {type}", ex);
        }

        public void LogInfo(string message)
        {
            Console.WriteLine($"Info: {message} in {type}");
        }

        public void LogWarning(string message)
        {
            Console.WriteLine($"Warning: {message} in {type}");
        }

        public void LogDebug(string message)
        {
            Console.WriteLine($"Debug: {message} in {type}");
        }

        public void Configure(Action<LogerOptions> configurer)
        {
            configurer(_options);
        }
    }

    public class LogerOptions
    {
        private readonly ServiceCollection _services = ServiceCollection.Instance;

        public LogerOptions()
        {
            try
            {
                var config = _services.GetService<IConfiguration>();

                var logLevel = config.Get("Environment", "LogLevel") as string;

                switch (logLevel)
                {
                    case "Info":
                        LogLevel = LogLevel.Info;
                        break;
                    case "Debug":
                        LogLevel = LogLevel.Debug;
                        break;
                    default:
                        break;
                }
            }
            catch
            {

            }
        }
        public LogLevel LogLevel {  get; set; } = LogLevel.Info;
    }
}

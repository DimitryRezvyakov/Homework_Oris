using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Common.Abstractions
{
    public interface Ilogger<T>
    {
        Type type { get; }
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(Exception ex);
        void LogFatal(string message, Exception ex);
    }
}

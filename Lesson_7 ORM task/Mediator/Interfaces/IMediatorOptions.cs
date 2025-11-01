using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Interfaces
{
    /// <summary>
    /// Configure mediator
    /// </summary>
    public interface IMediatorOptions
    {
        /// <summary>
        /// Assemblies in with will be searched for requests and handlers
        /// </summary>
        public Assembly[] Assemblies { get; set; }

        /// <summary>
        /// Throw exception if handler was not found
        /// </summary>
        public bool ThrowIfNotFound { get; set; }
    }
}

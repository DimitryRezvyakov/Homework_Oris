using Mediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Options
{
    public class MediatorOptions : IMediatorOptions
    {
        /// <summary>
        /// Configure itself
        /// </summary>
        /// <param name="configurator">Configuring action</param>
        public MediatorOptions(Action<IMediatorOptions>? configurator = null)
        {
            configurator?.Invoke(this);
        }

        /// <summary>
        /// Assemblies in which handlers and requests will be searhed
        /// </summary>
        public Assembly[] Assemblies { get; set; } = new[] { Assembly.GetExecutingAssembly() };

        /// <summary>
        /// Throw exception if handler was not found
        /// </summary>
        public bool ThrowIfNotFound { get; set; } = false;
    }
}

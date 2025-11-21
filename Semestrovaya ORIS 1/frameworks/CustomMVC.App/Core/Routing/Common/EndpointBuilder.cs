using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Core.Routing.Common
{
    /// <summary>
    /// Default enpoint builder
    /// </summary>
    public abstract class EndpointBuilder
    {
        /// <summary>
        /// Endpoint metadata
        /// </summary>
        public List<object> Metadata { get; set; } = new();

        public abstract Endpoint Build();
    }
}

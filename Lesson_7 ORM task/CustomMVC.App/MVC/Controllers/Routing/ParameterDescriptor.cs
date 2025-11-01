using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Routing
{
    public class ParameterDescriptor
    {
        /// <summary>
        /// Parameter name
        /// </summary>
        public virtual string Name { get; set; } = null!;

        /// <summary>
        /// Parameter type
        /// </summary>
        public virtual Type ParameterType { get; set; } = null!;

        /// <summary>
        /// Default value for a parameter
        /// </summary>
        public object? DefaultValue { get; set; }

        /// <summary>
        /// Binding attributes for parameter
        /// </summary>
        public IReadOnlyList<object> ParameterAttributes { get; set; } = [];

        /// <summary>
        /// Parameter binding info
        /// </summary>
        public BindingInfo? BindingInfo { get; set; }

        public ParameterDescriptor(ParameterInfo p)
        {
            Name = p.Name ?? string.Empty;
            ParameterType = p.ParameterType;
            DefaultValue = p.HasDefaultValue ? p.DefaultValue : null;
            ParameterAttributes = p.GetCustomAttributes(inherit: true).ToList();
            BindingInfo = new BindingInfo(p);
        }

        public ParameterDescriptor() { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORMLibrary.Attributes
{
    /// <summary>
    /// Metadata
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyAttribute : Attribute
    {
        public KeyAttribute() { }
    }
}

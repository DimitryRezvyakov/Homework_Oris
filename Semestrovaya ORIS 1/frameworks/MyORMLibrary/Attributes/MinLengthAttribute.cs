using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORMLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MinLengthAttribute : Attribute
    {
        public int MinLength { get; set; }
        public MinLengthAttribute(int minLength)
        {
            MinLength = minLength;
        }
    }
}

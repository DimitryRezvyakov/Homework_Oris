using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORMLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RangeAttribute : Attribute
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public RangeAttribute(int minValue, int maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORMLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignKeyAttribute : Attribute
    {
        public string ForeignKeyPropertyName { get; set; }
        public ForeignKeyAttribute(string foreignKeyPropertyName)
        {
            ForeignKeyPropertyName = foreignKeyPropertyName;
        }
    }
}

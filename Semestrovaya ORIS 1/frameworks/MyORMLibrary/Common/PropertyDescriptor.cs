using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyORMLibrary.Common
{
    public class PropertyDescriptor
    {
        public PropertyDescriptor(PropertyInfo propertyInfo)
        {
            PropertyName = propertyInfo.Name;

            IsKey = propertyInfo.GetCustomAttribute<MyORMLibrary.Attributes.KeyAttribute>() != null;

            IsNotNull = propertyInfo.GetCustomAttribute<MyORMLibrary.Attributes.RequiredAttribute>() != null;

            if (propertyInfo.PropertyType == typeof(string))
            {
                var maxLength = propertyInfo.GetCustomAttribute<MyORMLibrary.Attributes.MaxLengthAttribute>()?.MaxLength ?? int.MaxValue;
                var minLength = propertyInfo.GetCustomAttribute<MyORMLibrary.Attributes.MinLengthAttribute>()?.MinLength ?? int.MinValue;

                if (maxLength != int.MaxValue)
                    SqlConstraints += $"VARCHAR({maxLength})";
                else
                    SqlConstraints += $"TEXT";

                List<string> checks = new();

                if (minLength > int.MinValue)
                    checks.Add($"char_length(\"{PropertyName}\") >= {minLength}");
                if (maxLength != int.MaxValue)
                    checks.Add($"char_length(\"{PropertyName}\") <= {maxLength}");

                if (checks.Any())
                    SqlConstraints += $" CHECK ({string.Join(" AND ", checks)})";
            }

            else
            {
                var rangeAttr = propertyInfo.GetCustomAttribute<MyORMLibrary.Attributes.RangeAttribute>();
                var maxValue = rangeAttr?.MaxValue ?? int.MaxValue;
                var minValue = rangeAttr?.MinValue ?? int.MinValue;

                string sqlType = GetSqlType(propertyInfo.PropertyType);
                SqlConstraints += sqlType;

                if (rangeAttr != null)
                    SqlConstraints += $" CHECK (\"{PropertyName}\" >= {minValue} AND \"{PropertyName}\" <= {maxValue})";
            }

            if (IsNotNull)
                SqlConstraints += " NOT NULL";

            if (IsKey)
                SqlConstraints += " PRIMARY KEY";
        }

        public string PropertyName { get; set; }
        public string SqlConstraints { get; set; } = "";
        public bool IsNotNull { get; set; } = false;
        public bool IsKey { get; set; } = false;


        private string GetSqlType(Type type)
        {
            if (type == typeof(int)) return "INT";
            if (type == typeof(long)) return "BIGINT";
            if (type == typeof(float)) return "REAL";
            if (type == typeof(double)) return "DOUBLE PRECISION";
            if (type == typeof(decimal)) return "NUMERIC";
            if (type == typeof(bool)) return "BOOLEAN";
            if (type == typeof(DateTime)) return "TIMESTAMP";
            if (type == typeof(Guid)) return "UUID";
            if (type == typeof(string)) return "TEXT";
            return "TEXT";
        }
    }
}

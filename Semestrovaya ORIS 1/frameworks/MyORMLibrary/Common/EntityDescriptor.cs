using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyORMLibrary.Attributes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyORMLibrary.Common
{
    public class EntityDescriptor
    {
        public string TableName { get; set; }
        public string? Key { get; set; }
        public Dictionary<EntityDescriptor, string> ForeingKeys { get; set; }
        public PropertyDescriptor[] PropertyDescriptors { get; set; }

        public EntityDescriptor(Type type)
        {
            var props = type.GetProperties().ToList();

            var scalarProps = props
                .Where(p =>!p.PropertyType.IsClass || p.PropertyType == typeof(string) || p.PropertyType == typeof(Guid) || p.PropertyType == typeof(DateTime))
                .Where(p => !typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType) || p.PropertyType == typeof(string))
                .ToList();

            TableName = type.GetCustomAttribute<MyORMLibrary.Attributes.TableAttribute>()?.TableName ?? type.Name;

            Key = props.FirstOrDefault(p => p.GetCustomAttribute<MyORMLibrary.Attributes.KeyAttribute>() != null)?.Name ?? 
                props.FirstOrDefault(p => p.Name.Contains("Id", StringComparison.OrdinalIgnoreCase))?.Name;

            ForeingKeys = props.Where(p => p.GetCustomAttribute<MyORMLibrary.Attributes.ForeignKeyAttribute>() != null)
                .Select(p =>
                new
                {
                    Key = new EntityDescriptor(p.PropertyType),
                    Value = p.GetCustomAttribute<MyORMLibrary.Attributes.ForeignKeyAttribute>()!.ForeignKeyPropertyName
                })
            .ToDictionary(obj => obj.Key, obj => obj.Value);

            PropertyDescriptors = scalarProps.Select(p => new PropertyDescriptor(p)).ToArray();
        }
    }
}

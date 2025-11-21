using MyORMLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORMLibrary.Core
{
    public static class EntityMapper
    {
        public static string CreateTable(EntityDescriptor entityDescriptors)
        {
            var tableName = entityDescriptors.TableName;

            var rows = entityDescriptors.PropertyDescriptors.Select(
                p =>
                {
                    var propName = p.PropertyName;
                    return $"\"{propName}\" {p.SqlConstraints}";
                });

            return $"CREATE TABLE IF NOT EXISTS \"{tableName}\" ({string.Join(", ", rows)});";
        }
    }
}

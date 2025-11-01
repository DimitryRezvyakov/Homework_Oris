using Microsoft.Data.SqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORMLibrary.Common
{
    public static class Mapper
    {
        public static T Map<T>(NpgsqlDataReader reader) where T : class, new()
        {
            var instance = (T)Activator.CreateInstance(typeof(T))!;
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                {
                    var value = reader[property.Name];

                    property.SetValue(instance, value);
                }
            }

            return instance;
        }
    }
}

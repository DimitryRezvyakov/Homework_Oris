using Microsoft.Data.SqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyORMLibrary.Common
{
    public static class Mapper
    {

        public static T Map<T>(NpgsqlDataReader reader) where T : class, new()
        {
            var instance = new T();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                try
                {
                    var ordinal = reader.GetOrdinal(property.Name);

                    if (reader.IsDBNull(ordinal))
                    {
                        if (property.PropertyType == typeof(string[]))
                            property.SetValue(instance, Array.Empty<string>());
                        else if (property.PropertyType == typeof(List<string>))
                            property.SetValue(instance, new List<string>());
                        continue;
                    }

                    var value = reader.GetValue(ordinal);

                    if ((property.PropertyType == typeof(List<string>) ||
                         property.PropertyType == typeof(string[])) &&
                        value is string jsonString)
                    {
                        var list = JsonSerializer.Deserialize<List<string>>(jsonString) ?? new List<string>();
                        if (property.PropertyType == typeof(string[]))
                            property.SetValue(instance, list.ToArray());
                        else
                            property.SetValue(instance, list);
                    }
                    else
                    {
                        property.SetValue(instance, value);
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    continue;
                }
            }

            return instance;
        }
    }
}

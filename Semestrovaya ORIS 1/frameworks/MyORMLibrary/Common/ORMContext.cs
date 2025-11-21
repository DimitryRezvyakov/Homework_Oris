using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MyORMLibrary.Common
{
    public class ORMContext : IORMContext
    {
        private readonly string _connectionString;
        public ORMContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public T Create<T>(T entity) where T : class
        {
            var tableName = typeof(T).Name;

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var properties = typeof(T).GetProperties()
                    .Where(p => !p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase) &&
                               !"Id".Contains(p.Name))
                    .ToArray();

                var propertyNames = properties.Select(p => $"\"{p.Name}\"").ToArray();
                var propertyValues = properties.Select(p => $"@{p.Name}");

                string sql = $"INSERT INTO \"{tableName}\" ({string.Join(',', propertyNames)}) VALUES ({string.Join(',', propertyValues)})";

                NpgsqlCommand sqlCommand = new NpgsqlCommand(sql, connection);

                foreach (var prop in properties)
                {
                    var value = prop.GetValue(entity);

                    // Проверяем, является ли свойство массивом или списком строк
                    if (value != null && IsStringCollection(prop.PropertyType))
                    {
                        // Конвертируем в JSON и указываем тип jsonb
                        var json = System.Text.Json.JsonSerializer.Serialize(value);
                        var parameter = sqlCommand.Parameters.AddWithValue($"@{prop.Name}", NpgsqlTypes.NpgsqlDbType.Jsonb, json);
                    }
                    else
                    {
                        sqlCommand.Parameters.AddWithValue($"@{prop.Name}", value ?? DBNull.Value);
                    }
                }

                sqlCommand.ExecuteNonQuery();
            }

            return entity;
        }

        private static bool IsStringCollection(Type type)
        {
            if (type.IsArray && type.GetElementType() == typeof(string))
                return true;

            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                var elementType = type.GetGenericArguments()[0];

                if (elementType == typeof(string) &&
                    (genericType == typeof(List<>) ||
                     genericType == typeof(IList<>) ||
                     genericType == typeof(ICollection<>) ||
                     genericType == typeof(IEnumerable<>)))
                {
                    return true;
                }
            }

            return false;
        }

        public T ReadById<T>(int id) where T : class, new()
        {
            var tableName = typeof(T).Name;

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                var idPropName = typeof(T).GetProperties().First(p => p.Name.Contains("Id", StringComparison.OrdinalIgnoreCase)).Name;

                connection.Open();
                string sql = $"SELECT * FROM \"{tableName}\" WHERE \"{idPropName}\" = @id";
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        T instance = Mapper.Map<T>(reader);

                        return instance;
                    }
                }
            }
            return null;
        }

        public List<T> ReadByAll<T>() where T : class, new()
        {

            var tableName = typeof(T).Name;
            List<T> result = new();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string sql = $"SELECT * FROM \"{tableName}\"";
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        T instance = Mapper.Map<T>(reader);

                        result.Add(instance);
                    }
                }
            }

            return result;
        }

        public void Update<T>(int id, T entity)
        {
            var tableName = typeof(T).Name;
            var properties = typeof(T).GetProperties()
                                      .Where(p => p.Name != "Id")
                                      .ToList();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                var setProps = string.Join(", ", properties.Select(p => $"\"{p.Name}\" = @{p.Name}"));

                var sql = $"UPDATE \"{tableName}\" SET {setProps} WHERE \"Id\" = @Id";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    foreach (var prop in properties)
                    {
                        var value = prop.GetValue(entity) ?? DBNull.Value;
                        command.Parameters.AddWithValue($"@{prop.Name}", value);
                    }
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete<T>(int id)
        {
            var tableName = typeof(T).Name;
            var idPropName = typeof(T).GetProperties().First(p => p.Name.Contains("Id", StringComparison.OrdinalIgnoreCase)).Name;

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string sql = $"DELETE FROM \"{tableName}\" WHERE \"{idPropName}\" = @id";
                NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            }
        }

        public T UseSqlCommandQuery<T>(string sql, Dictionary<string, object> parameters = null)
            where T : class, new()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return Mapper.Map<T>(reader);
                        }
                    }
                }
            }

            return null;
        }

        public void UseSqlCommandNonQuery(string sql, Dictionary<string, object> parameters = null)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            // Проверяем, является ли свойство массивом или списком строк
                            if (param.Value != null && IsStringCollection(param.Value.GetType()))
                            {
                                // Конвертируем в JSON и указываем тип jsonb
                                var json = System.Text.Json.JsonSerializer.Serialize((List<string>)param.Value);
                                var parameter = command.Parameters.AddWithValue($"@{param.Key}", NpgsqlTypes.NpgsqlDbType.Jsonb, json);
                            }
                            else
                            {
                                command.Parameters.AddWithValue($"@{param.Key}", param.Value ?? DBNull.Value);
                            }

                            command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<T> UseSqlCommandQueryCollection<T>(string sql, Dictionary<string, object> parameters = null)
            where T : class, new()
        {
            var result = new List<T>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(Mapper.Map<T>(reader));
                        }
                    }
                }
            }

            return result;
        }
    }
}

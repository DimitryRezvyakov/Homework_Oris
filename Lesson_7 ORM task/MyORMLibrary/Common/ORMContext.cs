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
                    sqlCommand.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(entity) ?? DBNull.Value);
                }

                sqlCommand.ExecuteNonQuery();
            }

            return entity;
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
    }
}

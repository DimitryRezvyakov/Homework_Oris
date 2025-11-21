using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyORMLibrary.Common;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MyORMLibrary.Core
{
    public abstract class ORMContext
    {
        protected string ConnectionString;
        public ORMContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public virtual void OnModelCreating()
        {
            var entities = GetEntities();

            var entitiesWithForeingKeys = new List<EntityDescriptor>();

            foreach (var entity in entities)
            {
                var descriptor = new EntityDescriptor(entity);

                if (descriptor.ForeingKeys != null && descriptor.ForeingKeys.Count > 0)
                    entitiesWithForeingKeys.Add(descriptor);

                string sqlCreateTable = EntityMapper.CreateTable(descriptor);

                using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
                {
                    var command = new NpgsqlCommand(sqlCreateTable, connection);

                    command.ExecuteNonQuery();
                }
            }

            foreach (var entity in entitiesWithForeingKeys)
            {
                var tableName = entity.TableName;

                foreach ((var table, var fk)  in entity.ForeingKeys)
                {
                    using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
                    {
                        var sqlString = $"ALTER TABLE \"{tableName}\" ADD CONSTRAINT fk_{tableName}_{table.TableName}" +
                            $" FOREIGN KEY (\"{fk}\") REFERENCES \"{table.TableName}\" (\"{table.Key}\")";

                        var command = new NpgsqlCommand(sqlString, connection);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void EnsureCreated()
        {

        }

        private List<Type> GetEntities()
        {
            return this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(t => t.GetType().IsGenericType &&
                        t.GetType().GetGenericTypeDefinition() == typeof(DbSet<>))
                .Select(t => t.GetType().GetGenericArguments()[0])
                .ToList();
        }
    }
}

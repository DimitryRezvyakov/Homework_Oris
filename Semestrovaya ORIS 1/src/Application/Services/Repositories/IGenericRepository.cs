using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Repositories
{
    public interface IGenericRepository
    {
        public List<T> Entities<T>()
            where T : class, new();
        public Task<List<T>> GetAll<T>()
            where T : class, new();
        public Task<T> GetById<T>(int id)
            where T : class, new();
        public Task Create<T>(T entity)
            where T : class;
        public Task Update<T>(int id, T entity)
            where T : class, new();
        public Task Delete<T>(int id)
            where T : class;
        public Task UseSqlCommandNonQuery(string sql, Dictionary<string, object> parameters);
        public Task<T> UseSqlCommandQuery<T>(string sql, Dictionary<string, object> parameters)
            where T : class, new();
        public Task<List<T>> UseSqlCommandQueryCollection<T>(string sql, Dictionary<string, object> parameters)
            where T : class, new();
    }
}

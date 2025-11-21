using Application.Services.Repositories;
using MyORMLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class GenericRepository : IGenericRepository
    {
        private readonly IORMContext _context;

        public GenericRepository(IORMContext context)
        {
            _context = context;
        }

        public async Task Create<T>(T entity)
            where T : class
        {
            _context.Create(entity);

            await Task.CompletedTask;
        }

        public async Task Delete<T>(int id)
            where T: class 
        {
            _context.Delete<T>(id);

            await Task.CompletedTask;
        }

        public List<T> Entities<T>()
            where T : class, new()
        {
            return _context.ReadByAll<T>();
        }

        public Task<List<T>> GetAll<T>()
            where T : class, new()
        {
            return Task.FromResult<List<T>>(_context.ReadByAll<T>());
        }

        public Task<T> GetById<T>(int id)
            where T : class, new()
        {
            return Task.FromResult<T>(_context.ReadById<T>(id));
        }

        public async Task Update<T>(int id, T entity)
            where T : class, new()
        {
            _context.Update<T>(id, entity);

            await Task.CompletedTask;
        }
        public async Task UseSqlCommandNonQuery(string sql, Dictionary<string, object> parameters)
        {
            _context.UseSqlCommandNonQuery(sql, parameters);

            await Task.CompletedTask;
        }
        public Task<T> UseSqlCommandQuery<T>(string sql, Dictionary<string, object> parameters)
            where T : class, new()
        {
            return Task.FromResult<T>(_context.UseSqlCommandQuery<T>(sql, parameters));
        }

        public Task<List<T>> UseSqlCommandQueryCollection<T>(string sql, Dictionary<string, object> parameters)
            where T : class, new()
        {
            return Task.FromResult<List<T>>(_context.UseSqlCommandQueryCollection<T>(sql, parameters));
        }
    }
}

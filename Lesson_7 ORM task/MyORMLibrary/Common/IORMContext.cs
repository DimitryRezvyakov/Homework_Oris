using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORMLibrary.Common
{
    public interface IORMContext
    {
        public T ReadById<T>(int id) where T : class, new();
        public T Create<T>(T entity) where T : class;
        public List<T> ReadByAll<T>() where T : class, new();
        public void Update<T>(int id, T entity);
        public void Delete<T>(int id);
    }
}

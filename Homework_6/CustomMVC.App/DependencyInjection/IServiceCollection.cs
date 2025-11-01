using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.DependencyInjection
{
    public interface IServiceCollection
    {
        public Dictionary<Type, Type> Singleton { get;}
        public Dictionary<Type, Type> Transient { get;}
        public Dictionary<Type, Type> Scoped { get;}

        public T GetService<T>(object[]? parameters = null);
    }
}

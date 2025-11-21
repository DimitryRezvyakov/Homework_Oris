using CustomMVC.App.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.Common.Abstractions
{
    public interface IConfiguration
    {
        public object? Get(params string[] keys);
        public T? Get<T>(params string[] keys);
    }
}

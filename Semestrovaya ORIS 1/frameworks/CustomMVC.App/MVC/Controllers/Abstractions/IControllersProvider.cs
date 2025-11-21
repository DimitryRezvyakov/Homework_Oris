using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMVC.App.MVC.Controllers.Abstractions
{
    public interface IControllersProvider
    {
        private Dictionary<string, Type> _controllers { get => _controllers;  }

        public Type GetController(string name);

        public string[] GetControllersNames();

        public Type[] GetControllersTypes();
    }
}

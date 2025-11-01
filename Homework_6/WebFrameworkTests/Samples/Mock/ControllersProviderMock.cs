using CustomMVC.App.MVC.Controllers.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFrameworkTests.Samples.Mock
{
    public class ControllersProviderMock : IControllersProvider
    {
        private Dictionary<string, Type> _controllers = new Dictionary<string, Type>()
        {
            {"Home", typeof(HomeController)},
        };

        public Type GetController(string name)
        {
            return _controllers[name];
        }

        public string[] GetControllersNames()
        {
            return _controllers.Keys.ToArray();
        }

        public Type[] GetControllersTypes()
        {
            return _controllers.Values.ToArray();
        }
    }
}

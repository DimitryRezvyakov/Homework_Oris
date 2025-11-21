using MyORMLibrary.Common;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORMLibrary.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void AddORMContext(this IServiceProviderCustom serviceProvider, string connectionString)
        {
            serviceProvider.AddSingleton<IORMContext, ORMContext>(new object[] { connectionString });
        }
    }
}

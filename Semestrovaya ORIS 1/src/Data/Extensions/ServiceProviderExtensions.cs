using Application.Services.Repositories;
using Data.Repositories;
using MyORMLibrary.Extensions;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void AddDAL(this IServiceProviderCustom serviceProvider, string connectionString)
        {
            serviceProvider.AddORMContext(connectionString);
            serviceProvider.AddScoped<IGenericRepository, GenericRepository>();
        }
    }
}

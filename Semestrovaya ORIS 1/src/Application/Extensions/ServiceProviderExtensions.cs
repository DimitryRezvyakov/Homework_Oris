using Application.Services.Repositories;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediator.Extensions;
using System.Reflection;

namespace Application.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void AddApplicationLayer(this IServiceProviderCustom serviceProvider)
        {
            serviceProvider.UseMediator(opt =>
            {
                opt.Assemblies = new Assembly[] { Assembly.GetExecutingAssembly() };
            });
        }
    }
}

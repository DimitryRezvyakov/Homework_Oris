using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
        public interface IServiceProviderCustom
        {
            /// <summary>
            /// Getting service
            /// </summary>
            /// <typeparam name="T">Service interface</typeparam>
            /// <param name="parameters">Ctor arguments</param>
            /// <returns>T</returns>
            public T GetService<T>(object[]? parameters = null);

            /// <summary>
            /// Adding transient service
            /// </summary>
            /// <typeparam name="TInterface">Interface</typeparam>
            /// <typeparam name="TImplimentation">Implimentation</typeparam>
            /// <param name="parameters">Ctror arguments</param>
            public void AddTransient<TInterface, TImplimentation>(object[]? parameters = null) where TImplimentation : class, TInterface;

            /// <summary>
            /// Adding scoped service
            /// </summary>
            /// <typeparam name="TInterface">Interface</typeparam>
            /// <typeparam name="TImplimentation">Implimentation</typeparam>
            /// <param name="parameters">Ctror arguments</param>
            public void AddScoped<TInterface, TImplimentation>(object[]? parameters = null) where TImplimentation : class, TInterface;

            /// <summary>
            /// Adding singleton service
            /// </summary>
            /// <typeparam name="TInterface">Interface</typeparam>
            /// <typeparam name="TImplimentation">Implimentation</typeparam>
            /// <param name="parameters">Ctror arguments</param>
            public void AddSingleton<TInterface, TImplimentation>(object[]? parameters = null) where TImplimentation : class, TInterface;
    }
}

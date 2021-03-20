using System;
using Microsoft.Extensions.DependencyInjection;

namespace GbUtil.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddScopedIf<TService>(
            this IServiceCollection services,
            bool condition,
            Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (implementationFactory == null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            if (condition)
            {
                return services.AddScoped(typeof(TService), implementationFactory);
            }

            return services;
        }

        public static IServiceCollection AddTransientIf<TService, TImplementation>(
            this IServiceCollection services,
            bool condition)
            where TService : class
            where TImplementation : class, TService
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (condition)
            {
                return services.AddTransient(typeof(TService), typeof(TImplementation));
            }

            return services;
        }
    }
}

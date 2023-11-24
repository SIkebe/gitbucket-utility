using Microsoft.Extensions.DependencyInjection;

namespace GbUtil.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddScopedIf<TService>(
        this IServiceCollection services,
        bool condition,
        Func<IServiceProvider, TService> implementationFactory)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(implementationFactory);

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
        ArgumentNullException.ThrowIfNull(services);

        if (condition)
        {
            return services.AddTransient(typeof(TService), typeof(TImplementation));
        }

        return services;
    }
}

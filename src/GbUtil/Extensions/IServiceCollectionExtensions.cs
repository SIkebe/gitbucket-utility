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

        return condition ? services.AddScoped(implementationFactory) : services;
    }

    public static IServiceCollection AddTransientIf<TService, TImplementation>(
        this IServiceCollection services,
        bool condition)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(services);

        return condition ? services.AddTransient<TService, TImplementation>() : services;
    }
}

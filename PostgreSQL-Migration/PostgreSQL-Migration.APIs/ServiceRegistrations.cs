using Microsoft.Extensions.DependencyInjection.Extensions;
using PostgreSQL_Migration.APIs.Infras;
using System.Reflection;

namespace PostgreSQL_Migration.APIs;

public static class ServiceRegistrations
{
    public static IServiceCollection AddRequiredServices(this IServiceCollection services, Assembly assembly, Action<IInfrastructureBuilder> appConfig)
    {
        AddDefaultServices<InfrastructureBuilder>(services, assembly, appConfig);

        return services;
    }

    static void AddDefaultServices<TInfrastructureBuilder>(IServiceCollection services, Assembly runtimeAssembly, Action<TInfrastructureBuilder>? appConfig = null)
        where TInfrastructureBuilder : InfrastructureBuilder
    {
        if (Activator.CreateInstance(typeof(TInfrastructureBuilder), services, runtimeAssembly) is TInfrastructureBuilder builder)
        {
            appConfig?.Invoke(builder);
        }
    }

    public static IServiceCollection AddEventHandlers(this IInfrastructureBuilder builder)
    {
        var selector = builder.EntryAssembly.DefinedTypes
            .Select(type => new
            {
                type,
                interfaces = type.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEventHandler<>)).ToArray()
            })
            .Where(x => x.interfaces.Any())
            .ToArray();

        var services = builder.Services;

        foreach (var type in selector)
        {
            foreach (var @interface in type.interfaces)
            {
                services.TryAddScoped(@interface, type.type);
            }
        }

        services.TryAddSingleton<IEventFactory>(provider =>
        {
            var factory = new EventFactory(provider);

            foreach (var type in selector)
            {
                foreach (var @interface in type.interfaces)
                {
                    factory.AddEvent(@interface.GetGenericArguments()[0]);
                }
            }

            return factory;
        });

        return services;
    }
}

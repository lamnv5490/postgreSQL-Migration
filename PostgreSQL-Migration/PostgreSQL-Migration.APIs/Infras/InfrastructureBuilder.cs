using System.Reflection;

namespace PostgreSQL_Migration.APIs.Infras;

public class InfrastructureBuilder : IInfrastructureBuilder
{
    public InfrastructureBuilder(IServiceCollection services, Assembly entryAssembly)
    {
        Services = services;
        EntryAssembly = entryAssembly;
    }

    public Assembly EntryAssembly { get; }

    public IServiceCollection Services { get; }
}

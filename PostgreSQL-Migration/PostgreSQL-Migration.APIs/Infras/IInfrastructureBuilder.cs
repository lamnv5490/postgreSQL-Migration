using System.Reflection;

namespace PostgreSQL_Migration.APIs.Infras;

public interface IInfrastructureBuilder
{
    Assembly EntryAssembly { get; }

    IServiceCollection Services { get; }
}

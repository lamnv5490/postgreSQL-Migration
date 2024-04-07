using System.Collections;

namespace PostgreSQL_Migration.APIs.Infras;

public interface IEventFactory
{
    Type? GetEventType(string eventName);

    IEnumerable<string> GetEvents();

    IServiceScope GetHandlers(Type eventType, out IEnumerable handlers);
}

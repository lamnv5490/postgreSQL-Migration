using PostgreSQL_Migration.APIs.Helpers;
using System.Collections;

namespace PostgreSQL_Migration.APIs.Infras;

class EventFactory : IEventFactory
{
    readonly List<Type> _eventTypes;

    public EventFactory(IServiceProvider service)
    {
        _eventTypes = new List<Type>();
        Service = service;
    }

    internal IServiceProvider Service { get; }

    public Type? GetEventType(string eventName) => _eventTypes?.FirstOrDefault(x => x.Name == eventName);

    public IEnumerable<string> GetEvents() => _eventTypes?.Select(x => x.GetDefinition()) ?? Array.Empty<string>();

    public IEnumerable GetHandlers(Type eventType) => Service.CreateScope().ServiceProvider.GetServices(typeof(IEventHandler<>).MakeGenericType(eventType));

    public IServiceScope GetHandlers(Type eventType, out IEnumerable handlers)
    {
        var scope = Service.CreateScope();
        handlers = scope.ServiceProvider.GetServices(typeof(IEventHandler<>).MakeGenericType(eventType));
        return scope;
    }

    public void AddEvent(Type eventType)
    {
        if (!_eventTypes.Contains(eventType))
        {
            _eventTypes.Add(eventType);
        }
    }
}

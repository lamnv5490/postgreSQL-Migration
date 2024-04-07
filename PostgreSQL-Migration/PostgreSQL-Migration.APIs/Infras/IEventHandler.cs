namespace PostgreSQL_Migration.APIs.Infras;

public interface IEventHandler<in TEvent> where TEvent : IEvent
{
    Task Handle(TEvent notification, CancellationToken cancellationToken);
}

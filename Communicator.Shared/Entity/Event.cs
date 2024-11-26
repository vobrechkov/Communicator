namespace Communicator.Shared.Entity;

public interface IDomainEvent : INotification
{
    public DateTime TimeStamp { get; set; }
}

public interface IDomainEventContext
{
    IEnumerable<IDomainEvent> GetDomainEvents();
}

public abstract class EventBase : IDomainEvent
{
    public string EventType => GetType().FullName;

    public DateTime TimeStamp { get; set; } = DateTimeOffset.Now.UtcDateTime;
}

public class EventEnvelope(IDomainEvent @event) : INotification
{
    public IDomainEvent Event { get; } = @event;
}
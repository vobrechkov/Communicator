using Communicator.Shared.Entity;

namespace Communicator.Shared.IntegrationEvents;

public interface IIntegrationEvent : INotification
{
    public string Id { get; set; }
    public DateTime TimeStamp { get; set; }
}

public interface ITenantIntegrationEvent : IIntegrationEvent, INotification
{
    public string TenantId { get; set; }
}
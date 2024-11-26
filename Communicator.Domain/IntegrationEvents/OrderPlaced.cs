using Communicator.Shared.Entity;
using Communicator.Shared.IntegrationEvents;

namespace Communicator.Domain.IntegrationEvents;

public record OrderPlaced : ITenantIntegrationEvent
{
    public string Id { get; set; } = NanoId.NewId();
    public string TenantId { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    public string OrderId { get; init; } = NanoId.NewId();
}
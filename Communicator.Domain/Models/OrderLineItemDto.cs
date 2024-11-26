namespace Communicator.Domain.Models;

public record OrderLineItemDto
{
    public Guid LineItemId { get; init; } = Guid.NewGuid();
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public string Description { get; init; } = string.Empty;
}
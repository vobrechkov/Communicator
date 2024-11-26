namespace Communicator.Shared.Entity;

#region Contracts

public interface IEntity
{
    public NanoId Id { get; set; }
}

public interface IAuditableEntity : IEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public interface ITenantEntity : IEntity
{
    public NanoId TenantId { get; set; }
}

public interface ITenantAuditableEntity : IAuditableEntity
{
    public NanoId TenantId { get; set; }
}

public interface IAggregateRoot : IEntity
{
    public HashSet<IDomainEvent> DomainEvents { get; }
}

public interface ITenantAggregateRoot : ITenantEntity, IAggregateRoot;

#endregion

#region Implementations

public abstract class EntityBase : IEntity
{
    public NanoId Id { get; set; } = NanoId.NewId();
}

public abstract class AuditableEntityBase : EntityBase, IAuditableEntity
{
    public DateTime CreatedAt { get; set; } = DateTimeOffset.Now.UtcDateTime;
    public DateTime? UpdatedAt { get; set; }
}

public abstract class TenantEntityBase : EntityBase
{
    public NanoId TenantId { get; set; } = NanoId.NewId();
}

public abstract class TenantAuditableEntityBase : TenantEntityBase, ITenantAuditableEntity
{
    public DateTime CreatedAt { get; set; } = DateTimeOffset.Now.UtcDateTime;
    public DateTime? UpdatedAt { get; set; }
}

#endregion

/// <summary>
/// ref: https://github.com/dotnet/eShop/blob/main/src/Ordering.Domain/SeedWork/ValueObject.cs
/// </summary>
public abstract class ValueObject
{
    protected static bool EqualOperator(ValueObject left, ValueObject right)
    {
        if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
        {
            return false;
        }
        return ReferenceEquals(left, null) || left.Equals(right);
    }

    protected static bool NotEqualOperator(ValueObject left, ValueObject right)
    {
        return !(EqualOperator(left, right));
    }

    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }

    public ValueObject GetCopy()
    {
        return MemberwiseClone() as ValueObject;
    }
}

public static class AggregateRootExtensions
{
    public static async Task RelayAndPublishEvents(this IAggregateRoot aggregateRoot, IPublisher publisher, CancellationToken cancellationToken = default)
    {
        if (aggregateRoot.DomainEvents is not null)
        {
            var @events = new IDomainEvent[aggregateRoot.DomainEvents.Count];
            aggregateRoot.DomainEvents.CopyTo(@events);
            aggregateRoot.DomainEvents.Clear();

            foreach (var @event in @events)
            {
                await publisher.Publish(new EventEnvelope(@event), cancellationToken);
            }
        }
    }
}
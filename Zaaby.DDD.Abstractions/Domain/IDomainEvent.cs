namespace Zaaby.DDD.Abstractions.Domain
{
    public interface IDomainEvent
    {
    }

    public interface IDomainEvent<out T> : IDomainEvent, IEntity<T>
    {
    }

    /// <summary>
    /// Marker interface for entities with <see cref="T:System.Guid" /> keys
    /// </summary>
    public interface IDomainEventWithGuidKey : IDomainEvent, IEntityWithGuidKey
    {
    }

    /// <summary>
    /// Marker interface for entities with <see cref="T:System.String" /> keys
    /// </summary>
    public interface IDomainEventWithStringKey : IDomainEvent, IEntityWithStringKey
    {
    }

    /// <summary>
    /// Marker interface for entities with <see cref="T:System.Int32" /> keys
    /// </summary>
    public interface IDomainEventWithIntKey : IDomainEvent, IEntityWithIntKey
    {
    }

    /// <summary>
    /// Marker interface for entities with <see cref="T:System.Int64" /> keys
    /// </summary>
    public interface IDomainEventWithLongKey : IDomainEvent, IEntityWithLongKey
    {
    }
}
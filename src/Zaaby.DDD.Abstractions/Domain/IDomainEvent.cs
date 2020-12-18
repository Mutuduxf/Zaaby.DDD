namespace Zaaby.DDD.Abstractions.Domain
{
    public interface IDomainEvent
    {
    }

    public interface IDomainEvent<out TId> : IDomainEvent, IValueObject
    {
        TId Id { get; }
    }
}
namespace Zaaby.DDD.Abstractions.Domain
{
    public interface IDomainEvent
    {
    }

    public interface IDomainEvent<out T> : IDomainEvent, IValueObject
    {
        T Id { get; }
    }
}
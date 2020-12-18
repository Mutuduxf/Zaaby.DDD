namespace Zaaby.DDD.Abstractions.Domain
{
    public record DomainEvent : ValueObject, IDomainEvent
    {

    }

    public record DomainEvent<TId> : IDomainEvent<TId>
    {
        public TId Id { get; }
    }
}
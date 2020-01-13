namespace Zaaby.DDD.Abstractions.Domain
{
    public interface IDomainEventHandler
    {
    }

    public interface IDomainEventHandler<in TDomainEvent> : IDomainEventHandler where TDomainEvent : IDomainEvent
    {
        void Handle(TDomainEvent domainEvent);
    }
}
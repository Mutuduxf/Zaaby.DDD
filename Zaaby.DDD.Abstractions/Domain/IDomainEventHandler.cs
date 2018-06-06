namespace Zaaby.DDD.Abstractions.Domain
{
    public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : DomainEvent
    {
        void Handle(TDomainEvent domainEvent);
    }
}
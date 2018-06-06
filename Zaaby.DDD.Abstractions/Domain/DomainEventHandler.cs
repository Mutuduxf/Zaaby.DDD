using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace Zaaby.DDD.Abstractions.Domain
{
    public abstract class DomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
        where TDomainEvent : DomainEvent
    {
        public DomainEventHandler(IEventBus eventBus)
        {
            eventBus.SubscribeEvent<TDomainEvent>(Handle);
        }

        public abstract void Handle(TDomainEvent domainEvent);
    }
}
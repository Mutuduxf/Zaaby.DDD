using OrderDomain.DomainEvents;
using Zaaby.DDD.Abstractions.Domain;
using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace OrderDomain.DomainServices.EventHandlers
{
    public class ReceivedOrderEventHandler : DomainEventHandler<ReceivedOrderEvent>
    {
        public ReceivedOrderEventHandler(IEventBus eventBus) : base(eventBus)
        {
        }

        public override void Handle(ReceivedOrderEvent domainEvent)
        {

        }
    }
}
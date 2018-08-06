using OrderDomain.DomainEvents;
using Zaaby.DDD.Abstractions.Domain;

namespace OrderDomain.DomainServices.EventHandlers
{
    public class ReceivedOrderEventHandler : IDomainEventHandler<ReceivedOrderEvent>
    {
        public void Handle(ReceivedOrderEvent domainEvent)
        {
            var ii = 123123213;
        }
    }
}
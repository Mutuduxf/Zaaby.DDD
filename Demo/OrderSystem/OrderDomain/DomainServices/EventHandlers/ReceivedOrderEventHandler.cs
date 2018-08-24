using OrderDomain.DomainEvents;
using Zaaby.DDD.Abstractions.Domain;

namespace OrderDomain.DomainServices.EventHandlers
{
    public class ReceivedOrderEventHandler : IDomainEventHandler<ReceivedOrderEvent>
    {
        public void Handle(ReceivedOrderEvent domainEvent)
        {
            var i = this.GetHashCode();
        }
    }
}
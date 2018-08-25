using AppleDomain.DomainEvents;
using Zaaby.DDD.Abstractions.Domain;
using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace AppleDomain.DomainServices
{
    public class AppleDomainService : IDomainService
    {
        private readonly IDomainEventPublisher _domainEventPublisher;

        public AppleDomainService(IDomainEventPublisher domainEventPublisher)
        {
            _domainEventPublisher = domainEventPublisher;
        }

        public void PublishDomainEventTest()
        {
            _domainEventPublisher.PublishEvent(new AppleDomainEvent());
        }

    }
}
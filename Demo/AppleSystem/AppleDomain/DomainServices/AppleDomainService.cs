using AppleDomain.DomainEvents;
using AppleDomain.IRepositories;
using Zaaby.DDD.Abstractions.Domain;
using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace AppleDomain.DomainServices
{
    public class AppleDomainService : IDomainService
    {
        private readonly IDomainEventPublisher _domainEventPublisher;
        public readonly IAppleRepository AppleRepository;

        public AppleDomainService(IDomainEventPublisher domainEventPublisher, IAppleRepository appleRepository)
        {
            _domainEventPublisher = domainEventPublisher;
            AppleRepository = appleRepository;
        }

        public void PublishDomainEventTest()
        {
            _domainEventPublisher.PublishEvent(new AppleDomainEvent());
        }
    }
}
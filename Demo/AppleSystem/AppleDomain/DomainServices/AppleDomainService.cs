using System.Collections.Generic;
using AppleDomain.AggregateRoots;
using AppleDomain.DomainEvents;
using AppleDomain.IRepositories;
using Zaaby.DDD.Abstractions.Domain;
using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace AppleDomain.DomainServices
{
    public class AppleDomainService : IDomainService
    {
        private readonly IDomainEventPublisher _domainEventPublisher;
        private readonly IAppleRepository _appleRepository;

        public AppleDomainService(IDomainEventPublisher domainEventPublisher, IAppleRepository appleRepository)
        {
            _domainEventPublisher = domainEventPublisher;
            _appleRepository = appleRepository;
        }

        public void PublishDomainEventTest()
        {
            _domainEventPublisher.PublishEvent(new AppleDomainEventA());
            _domainEventPublisher.PublishEvent(new AppleDomainEventB());
            var i = _appleRepository.GetHashCode();
        }

        public int AddApple(List<Apple> apples)
        {
            apples.ForEach(apple => _appleRepository.AddRdb(apple));
            return apples.Count;
        }
    }
}
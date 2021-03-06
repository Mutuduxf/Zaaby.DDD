using System.Collections.Generic;
using System.Threading.Tasks;
using AppleDomain.Aggregates;
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

        public async Task PublishDomainEventTest()
        {
            _domainEventPublisher.Publish(new AppleDomainEventA());
            await _domainEventPublisher.PublishAsync(new AppleDomainEventB());
            var i = _appleRepository.GetHashCode();
        }

        public int AddRdbApple(List<Apple> apples)
        {
            apples.ForEach(apple => _appleRepository.AddRdb(apple));
            return apples.Count;
        }

        public int AddMongoApple(List<Apple> apples)
        {
            apples.ForEach(apple => _appleRepository.AddMongo(apple));
            return apples.Count;
        }
    }
}
using AppleDomain.DomainEvents;
using AppleDomain.IRepositories;
using Zaaby.DDD.Abstractions.Domain;

namespace AppleDomain.DomainServices.EventHandlers
{
    public class TestDomainEventHandleA : IDomainEventHandler<AppleDomainEventA>, IDomainEventHandler<AppleDomainEventB>
    {
        public readonly IAppleRepository AppleRepository;

        public TestDomainEventHandleA(IAppleRepository appleRepository)
        {
            AppleRepository = appleRepository;
        }

        public void Handle(AppleDomainEventA domainEvent)
        {
            var i = AppleRepository.GetHashCode();
        }

        public void Handle(AppleDomainEventB domainEvent)
        {
            var i = AppleRepository.GetHashCode();
        }
    }
}
using AppleDomain.DomainEvents;
using AppleDomain.IRepositories;
using Zaaby.DDD.Abstractions.Domain;

namespace AppleDomain.DomainServices.EventHandlers
{
    public class TestDomainEventHandleB : IDomainEventHandler<AppleDomainEventA>
    {
        public IAppleRepository AppleRepository;

        public TestDomainEventHandleB(IAppleRepository appleRepository)
        {
            AppleRepository = appleRepository;
        }

        public void Handle(AppleDomainEventA domainEvent)
        {
            var i = AppleRepository.GetHashCode();
        }
    }
}
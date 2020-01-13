using AppleDomain.DomainServices;
using AppleDomain.IRepositories;
using IAppleApplication.IntegrationEvents;
using Zaaby.DDD.Abstractions.Application;

namespace AppleApplication
{
    public class TestIntegrationEventHandle : IIntegrationEventHandler<AppleIntegrationEvent>
    {
        private readonly AppleDomainService _appleDomainService;
        private readonly IAppleRepository _appleRepository;

        public TestIntegrationEventHandle(AppleDomainService appleDomainService, IAppleRepository appleRepository)
        {
            _appleDomainService = appleDomainService;
            _appleRepository = appleRepository;
        }

        public void Handle(AppleIntegrationEvent integrationEvent)
        {
            var i = GetHashCode();
            var i1 = _appleRepository.GetHashCode();
        }
    }
}
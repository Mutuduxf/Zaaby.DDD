using AppleDomain.DomainServices;
using IAppleApplication;
using IAppleApplication.IntegrationEvents;
using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace AppleApplication
{
    public class TestApplication : ITestApplication
    {
        private readonly AppleDomainService _appleDomainService;
        private readonly IIntegrationEventBus _integrationEventBus;

        public TestApplication(AppleDomainService appleDomainService, IIntegrationEventBus integrationEventBus)
        {
            _appleDomainService = appleDomainService;
            _integrationEventBus = integrationEventBus;
        }

        public void DomainEventTest()
        {
            _appleDomainService.PublishDomainEventTest();
        }

        public void IntegrationEventTest()
        {
            _integrationEventBus.PublishEvent(new AppleIntegrationEvent());
        }
    }
}
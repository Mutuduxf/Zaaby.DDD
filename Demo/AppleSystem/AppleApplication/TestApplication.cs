using System.Linq;
using AppleDomain.AggregateRoots;
using AppleDomain.DomainServices;
using IAppleApplication;
using IAppleApplication.IntegrationEvents;
using Zaabee.SequentialGuid;
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

        public int AddApple(int quantity)
        {
            var apples = Enumerable.Range(0, quantity)
                .Select(p => new Apple(SequentialGuidHelper.GenerateComb())).ToList();
            _appleDomainService.AddApple(apples);
            return quantity;
        }
    }
}
using System.Linq;
using AppleDomain.AggregateRoots;
using AppleDomain.DomainServices;
using IAppleApplication;
using IAppleApplication.IntegrationEvents;
using Zaabee.Dapper.UnitOfWork.Abstractions;
using Zaabee.SequentialGuid;
using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace AppleApplication
{
    public class TestApplication : ITestApplication
    {
        private readonly AppleDomainService _appleDomainService;
        private readonly IIntegrationEventBus _integrationEventBus;
        private readonly IZaabeeDbContext _zaabeeDbContext;

        public TestApplication(AppleDomainService appleDomainService, IIntegrationEventBus integrationEventBus,IZaabeeDbContext zaabeeDbContext)
        {
            _appleDomainService = appleDomainService;
            _integrationEventBus = integrationEventBus;
            _zaabeeDbContext = zaabeeDbContext;
        }

        public void DomainEventTest()
        {
            _appleDomainService.PublishDomainEventTest();
        }

        public void IntegrationEventTest()
        {
            _integrationEventBus.PublishEvent(new AppleIntegrationEvent());
        }

        public int AddRdbApple(int quantity)
        {
            _zaabeeDbContext.UnitOfWork.Begin();
            var apples = Enumerable.Range(0, quantity)
                .Select(p => new Apple(SequentialGuidHelper.GenerateComb())).ToList();
            _appleDomainService.AddRdbApple(apples);
            _zaabeeDbContext.UnitOfWork.Commit();
            return quantity;
        }

        public int AddMongoApple(int quantity)
        {
            var apples = Enumerable.Range(0, quantity)
                .Select(p => new Apple(SequentialGuidHelper.GenerateComb())).ToList();
            _appleDomainService.AddMongoApple(apples);
            return quantity;
        }
    }
}
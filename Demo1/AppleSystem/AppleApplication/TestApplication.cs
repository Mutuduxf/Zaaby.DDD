using System.Linq;
using AppleDomain.Aggregates;
using AppleDomain.DomainServices;
using IAppleApplication;
using IAppleApplication.IntegrationEvents;
using Zaabee.SequentialGuid;
using Zaaby.DDD.Abstractions.Infrastructure;
using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace AppleApplication
{
    public class TestApplication : ITestApplication
    {
        private readonly AppleDomainService _appleDomainService;
//        private readonly IIntegrationEventBus _integrationEventBus;
        private readonly IUnitOfWork _unitOfWork;

        public TestApplication(AppleDomainService appleDomainService,IUnitOfWork unitOfWork)
        {
            _appleDomainService = appleDomainService;
//            _integrationEventBus = integrationEventBus;
            _unitOfWork = unitOfWork;
        }

        public void DomainEventTest()
        {
            _appleDomainService.PublishDomainEventTest();
        }

        public void IntegrationEventTest()
        {
//            _integrationEventBus.Publish(new AppleIntegrationEvent());
        }

        public int AddRdbApple(int quantity)
        {
            _unitOfWork.Begin();
            var apples = Enumerable.Range(0, quantity)
                .Select(p => new Apple(SequentialGuidHelper.GenerateComb())).ToList();
            _appleDomainService.AddRdbApple(apples);
            _unitOfWork.Commit();
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
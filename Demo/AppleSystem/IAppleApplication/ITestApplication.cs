using Zaaby.DDD.Abstractions.Application;

namespace IAppleApplication
{
    public interface ITestApplication : IApplicationService
    {
        void DomainEventTest();
        void IntegrationEventTest();
        int AddRdbApple(int quantity);
        int AddMongoApple(int quantity);
    }
}
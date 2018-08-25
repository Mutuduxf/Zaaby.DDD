using Zaaby.DDD.Abstractions.Application;

namespace IAppleApplication
{
    public interface ITestApplication : IApplicationService
    {
        void DomainEventTest();
    }
}
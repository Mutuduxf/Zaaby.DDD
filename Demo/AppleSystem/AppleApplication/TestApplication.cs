using AppleDomain.DomainServices;
using IAppleApplication;

namespace AppleApplication
{
    public class TestApplication : ITestApplication
    {
        private readonly AppleDomainService _appleDomainService;
        
        public TestApplication(AppleDomainService appleDomainService)
        {
            _appleDomainService = appleDomainService;
        }
        
        public void DomainEventTest()
        {
            _appleDomainService.PublishDomainEventTest();
            _appleDomainService.PublishDomainEventTest();
        }
    }
}
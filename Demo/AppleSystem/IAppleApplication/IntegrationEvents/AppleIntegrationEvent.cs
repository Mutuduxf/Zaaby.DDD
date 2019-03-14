using Zaaby.DDD.Abstractions.Application;

namespace IAppleApplication.IntegrationEvents
{
    public class AppleIntegrationEvent : IIntegrationEvent
    {
        public TestEnum TestEnum { get; set; }
    }

    public enum TestEnum
    {
        A,B,C
    }
}
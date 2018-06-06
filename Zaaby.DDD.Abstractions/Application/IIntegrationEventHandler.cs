namespace Zaaby.DDD.Abstractions.Application
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> where TIntegrationEvent : IIntegrationEvent
    {
        void Handle(TIntegrationEvent domainEvent);
    }
}
namespace Zaaby.DDD.Abstractions.Infrastructure.EventBus
{
    public interface IDomainEventBus : IDomainEventPublisher, IDomainEventSubscriber
    {

    }
}
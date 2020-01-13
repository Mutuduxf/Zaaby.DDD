using System.Threading.Tasks;
using Zaaby.DDD.Abstractions.Domain;

namespace Zaaby.DDD.Abstractions.Infrastructure.EventBus
{
    public interface IDomainEventPublisher
    {
        void Publish<T>(T @event) where T : IDomainEvent;
        Task PublishAsync<T>(T @event) where T : IDomainEvent;
    }
}
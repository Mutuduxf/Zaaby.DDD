using System.Threading.Tasks;
using Zaaby.DDD.Abstractions.Application;

namespace Zaaby.DDD.Abstractions.Infrastructure.EventBus
{
    public interface IIntegrationEventPublisher
    {
        void Publish<T>(T integrationEvent) where T : IIntegrationEvent;
        Task PublishAsync<T>(T integrationEvent) where T : IIntegrationEvent;
    }
}
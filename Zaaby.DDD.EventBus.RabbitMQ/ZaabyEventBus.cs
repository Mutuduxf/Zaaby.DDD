using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Zaabee.RabbitMQ.Abstractions;
using Zaaby.DDD.Abstractions.Domain;
using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace Zaaby.DDD.EventBus.RabbitMQ
{
    public class ZaabyEventBus : IEventBus
    {
        private readonly IZaabeeRabbitMqClient _rabbitMqClient;
        private readonly IServiceProvider _serviceProvider;

        public ZaabyEventBus(IServiceProvider serviceProvider, IZaabeeRabbitMqClient rabbitMqClient)
        {
            _rabbitMqClient = rabbitMqClient;
            _serviceProvider = serviceProvider;
            var t = _serviceProvider.GetServices<IDomainEventHandler>().ToList();
        }

        public void PublishEvent<T>(T @event) where T : IEvent
        {
            _rabbitMqClient.PublishEvent(@event);
        }

        public void SubscribeEvent<T>(Action<T> handle) where T : IEvent
        {
            _rabbitMqClient.SubscribeEvent(handle);
        }
    }
}
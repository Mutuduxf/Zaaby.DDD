using System;
using Zaabee.RabbitMQ.Abstractions;
using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace Zaaby.DDD.EventBus.RabbitMQ
{
    public class ZaabyEventBus : IEventBus
    {
        private static IZaabeeRabbitMqClient _rabbitMqClient;

        public ZaabyEventBus(IZaabeeRabbitMqClient rabbitMqClient)
        {
            _rabbitMqClient = rabbitMqClient;
        }

        public void PublishEvent<T>(T @event) where T : IEvent
        {
            _rabbitMqClient.PublishEvent<T>(@event);
        }

        public void SubscribeEvent<T>(Action<T> handle) where T : IEvent
        {
            _rabbitMqClient.SubscribeEvent(handle);
        }
    }
}
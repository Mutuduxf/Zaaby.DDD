using System;
using System.Linq;
using Zaabee.RabbitMQ.Abstractions;
using Zaaby.DDD.Abstractions.Application;
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

            RegisterDomainEventSubscriber();
        }

        public void PublishEvent<T>(T @event) where T : IEvent
        {
            _rabbitMqClient.PublishEvent(@event);
        }

        public void SubscribeEvent<T>(Action<T> handle) where T : IEvent
        {
            _rabbitMqClient.SubscribeEvent(handle);
        }

        private void RegisterDomainEventSubscriber()
        {
            var integrationEventHandlerTypes = ZaabyServerExtension.AllTypes
                .Where(type => type.IsClass && typeof(IIntegrationEventHandler).IsAssignableFrom(type)).ToList();

            var subscribeMethod = _rabbitMqClient.GetType().GetMethod("SubscribeEvent");

            integrationEventHandlerTypes.ForEach(integrationEventHandlerType =>
            {
                subscribeMethod.MakeGenericMethod(integrationEventHandlerType).Invoke(_rabbitMqClient, null);
                
//                var i = _serviceProvider.GetService(integrationEventHandlerType);
//                var d1 = i is IIntegrationEventHandler<IIntegrationEvent>;
//                var d2 = i as IIntegrationEventHandler<IIntegrationEvent>;
//                var d3 = i is IIntegrationEventHandler;
//                var d4 = i as IIntegrationEventHandler;
//                var tt = i.GetType();
//                var i0 = tt.GetInterfaces();
//                var i1 = tt.BaseType?.IsGenericType;
//                var i2 = tt.BaseType?.IsGenericTypeDefinition;
//
//                var handleMethod = tt.GetMethod("Handle");
//                handleMethod.Invoke(i, new object[] {"hello"});
//
//
//
//
//                var r2 = (IIntegrationEventHandler<IIntegrationEvent>) i;
//
//                _rabbitMqClient.SubscribeEvent<IIntegrationEvent>(integrationEventHandlerType.FullName, p =>
//                {
//                    if (_serviceProvider.GetService(integrationEventHandlerType) is
//                        IIntegrationEventHandler<IIntegrationEvent>
//                        domainEventHandler)
//                        domainEventHandler.Handle(p);
//                });
            });
        }
    }
}
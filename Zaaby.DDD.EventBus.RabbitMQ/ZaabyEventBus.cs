using System;
using System.Linq;
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
            var domainEventHandlerTypes = ZaabyServerExtension.AllTypes
                .Where(type => type.IsClass && typeof(IDomainEventHandler).IsAssignableFrom(type)).ToList();

            domainEventHandlerTypes.ForEach(domainEventHandlerType =>
            {
                var i = _serviceProvider.GetService(domainEventHandlerType);
                var d1 = i is IDomainEventHandler<IDomainEvent>;
                var d2 = i as IDomainEventHandler<IDomainEvent>;
                var d3 = i is IDomainEventHandler;
                var d4 = i as IDomainEventHandler;
                var tt = i.GetType();
                var i0 = tt.GetInterfaces();
                var i1 = tt.BaseType?.IsGenericType;
                var i2 = tt.BaseType?.IsGenericTypeDefinition;

                var handleMethod = tt.GetMethod("Handle");
                handleMethod.Invoke(i,new object[] {"hello"});
                
                
                
                
                var r2 = (IDomainEventHandler<IDomainEvent>)i;
                
                _rabbitMqClient.SubscribeEvent<IDomainEvent>(domainEventHandlerType.FullName, p =>
                {
                    if (_serviceProvider.GetService(domainEventHandlerType) is IDomainEventHandler<IDomainEvent>
                        domainEventHandler)
                        domainEventHandler.Handle(p);
                });
            });
        }
    }
}
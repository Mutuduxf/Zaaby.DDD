using System;
using System.Linq;
using Zaaby.DDD.Abstractions.Domain;
using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace Zaaby.DDD
{
    public class DomainEventPublisher : IDomainEventPublisher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DomainEventHandlerProvider _domainEventHandlerProvider;

        public DomainEventPublisher(IServiceProvider serviceProvider,
            DomainEventHandlerProvider domainEventHandlerProvider)
        {
            _serviceProvider = serviceProvider;
            _domainEventHandlerProvider = domainEventHandlerProvider;
        }

        public void Subscribe<TDomainEvent>(Type handlerType) where TDomainEvent : IDomainEvent
        {
            _domainEventHandlerProvider.Register<TDomainEvent>(handlerType);
        }

        public void Subscribe(Type domainEventType, Type handlerType)
        {
            _domainEventHandlerProvider.Register(domainEventType, handlerType);
        }

        public void Reset()
        {
            _domainEventHandlerProvider.SubscriberResolves.Clear();
        }

        public void PublishEvent<T>(T domainEvent) where T : IDomainEvent
        {
            var type = typeof(T);
            if (!_domainEventHandlerProvider.SubscriberResolves.ContainsKey(type)) return;
            _domainEventHandlerProvider.SubscriberResolves[type].ForEach(handlerType =>
            {
                var handler = (IDomainEventHandler) _serviceProvider.GetService(handlerType);
                var handleMethodInfo = handlerType.GetMethods()
                    .First(m =>
                        m.Name == "Handle" &&
                        m.GetParameters().Count() == 1 &&
                        typeof(IDomainEvent).IsAssignableFrom(m.GetParameters()[0].ParameterType)
                    );
                handleMethodInfo.Invoke(handler, new object[] {domainEvent});
            });
        }
    }
}
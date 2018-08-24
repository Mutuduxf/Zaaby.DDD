using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Zaaby.DDD.Abstractions.Domain;
using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace Zaaby.DDD
{
    public class DomainEventPublisher : IDomainEventPublisher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventPublisher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RegisterIntegrationEventSubscriber();
        }

        private readonly ConcurrentDictionary<Type, List<Func<IDomainEventHandler<IDomainEvent>>>>
            _subscriberResolves = new ConcurrentDictionary<Type, List<Func<IDomainEventHandler<IDomainEvent>>>>();

        public void Subscribe<TDomainEvent>(Func<IDomainEventHandler> resolve) where TDomainEvent : IDomainEvent
        {
            var domainEventType = typeof(TDomainEvent);
            if (!_subscriberResolves.ContainsKey(domainEventType))
                _subscriberResolves.TryAdd(domainEventType, new List<Func<IDomainEventHandler<IDomainEvent>>>());
            _subscriberResolves[domainEventType].Add((Func<IDomainEventHandler<IDomainEvent>>) resolve);
        }

        public void Reset()
        {
            _subscriberResolves.Clear();
        }

        public void PublishEvent<T>(T @event) where T : IDomainEvent
        {
            var type = typeof(T);
            if (!_subscriberResolves.ContainsKey(type)) return;
            _subscriberResolves[type].ForEach(resolve => resolve().Handle(@event));
        }

        private void RegisterIntegrationEventSubscriber()
        {
            var domainEventHandlerTypes = ZaabyServerExtension.AllTypes
                .Where(type => type.IsClass && typeof(IDomainEventHandler).IsAssignableFrom(type)).ToList();

            domainEventHandlerTypes.ForEach(domainEventHandlerType =>
            {
                var handleMethod = domainEventHandlerType.GetMethods()
                    .First(m =>
                        m.Name == "Handle" &&
                        m.GetParameters().Count() == 1 &&
                        typeof(IDomainEvent).IsAssignableFrom(m.GetParameters()[0].ParameterType)
                    );
                var domainEventType = handleMethod.GetParameters()[0].ParameterType;
                if (!_subscriberResolves.ContainsKey(domainEventType))
                    _subscriberResolves.TryAdd(domainEventType,
                        new List<Func<IDomainEventHandler<IDomainEvent>>>());
                _subscriberResolves[domainEventType]
                    .Add(() => (IDomainEventHandler<IDomainEvent>) _serviceProvider.GetService(domainEventHandlerType));
            });
        }
    }
}
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
            RegisterDomainEventSubscriber();
        }

        private readonly ConcurrentDictionary<Type, List<Func<IDomainEventHandler>>>
            _subscriberResolves = new ConcurrentDictionary<Type, List<Func<IDomainEventHandler>>>();

        public void Subscribe<TDomainEvent>(Func<IDomainEventHandler> resolve) where TDomainEvent : IDomainEvent
        {
            var domainEventType = typeof(TDomainEvent);
            Subscribe(domainEventType, resolve);
        }

        public void Subscribe(Type domainEventType, Func<IDomainEventHandler> resolve)
        {
            if (!_subscriberResolves.ContainsKey(domainEventType))
                _subscriberResolves.TryAdd(domainEventType, new List<Func<IDomainEventHandler>>());
            _subscriberResolves[domainEventType].Add(resolve);
        }

        public void Reset()
        {
            _subscriberResolves.Clear();
        }

        public void PublishEvent<T>(T domainEvent) where T : IDomainEvent
        {
            var type = typeof(T);
            if (!_subscriberResolves.ContainsKey(type)) return;
            _subscriberResolves[type].ForEach(resolve =>
            {
                var handler = resolve();
                var handlerType = handler.GetType();
                var handleMethodInfo = handlerType.GetMethods()
                    .First(m =>
                        m.Name == "Handle" &&
                        m.GetParameters().Count() == 1 &&
                        typeof(IDomainEvent).IsAssignableFrom(m.GetParameters()[0].ParameterType)
                    );
                handleMethodInfo.Invoke(handler, new object[] {domainEvent});
            });
        }

        private void RegisterDomainEventSubscriber()
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
                Subscribe(domainEventType,
                    () => (IDomainEventHandler)_serviceProvider.GetService(domainEventHandlerType));
            });
        }
    }
}
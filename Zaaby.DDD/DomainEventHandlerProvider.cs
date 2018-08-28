using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Zaaby.DDD.Abstractions.Domain;

namespace Zaaby.DDD
{
    public class DomainEventHandlerProvider
    {
        internal readonly ConcurrentDictionary<Type, List<Type>>
            SubscriberResolves = new ConcurrentDictionary<Type, List<Type>>();

        public DomainEventHandlerProvider()
        {
            RegisterDomainEventSubscriber();
        }

        internal void Register<TDomainEvent>(Type handlerType) where TDomainEvent : IDomainEvent
        {
            var domainEventType = typeof(TDomainEvent);
            Register(domainEventType, handlerType);
        }

        internal void Register(Type domainEventType, Type handlerType)
        {
            if (!SubscriberResolves.ContainsKey(domainEventType))
                SubscriberResolves.TryAdd(domainEventType, new List<Type>());
            SubscriberResolves[domainEventType].Add(handlerType);
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
                Register(domainEventType, domainEventHandlerType);
            });
        }
    }
}
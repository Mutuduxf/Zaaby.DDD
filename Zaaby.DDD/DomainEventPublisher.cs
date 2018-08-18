using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Zaaby.DDD.Abstractions.Domain;

namespace Zaaby.DDD
{
    public class DomainEventPublisher
    {
        private readonly ConcurrentDictionary<Type, List<Func<IDomainEventHandler<IDomainEvent>>>>
            _subscriberResolves = new ConcurrentDictionary<Type, List<Func<IDomainEventHandler<IDomainEvent>>>>();

        public void Publish<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : IDomainEvent
        {
            var type = typeof(TDomainEvent);
            if (!_subscriberResolves.ContainsKey(type)) return;
            _subscriberResolves[type].ForEach(resolve => resolve().Handle(domainEvent));
        }

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
    }
}
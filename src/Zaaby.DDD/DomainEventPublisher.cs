using System;
using System.Threading.Tasks;
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

        public void Subscribe<TDomainEvent, THandler>()
            where TDomainEvent : IDomainEvent
            where THandler : IDomainEventHandler<TDomainEvent> =>
            _domainEventHandlerProvider.Register<TDomainEvent, THandler>();

        public void Subscribe<TDomainEvent>(Type handlerType) where TDomainEvent : IDomainEvent =>
            _domainEventHandlerProvider.Register<TDomainEvent>(handlerType);

        public void Subscribe(Type domainEventType, Type handlerType) =>
            _domainEventHandlerProvider.Register(domainEventType, handlerType);

        public void Reset() => _domainEventHandlerProvider.SubscriberResolves.Clear();

        public void Publish<T>(T domainEvent) where T : IDomainEvent
        {
            var type = typeof(T);
            if (!_domainEventHandlerProvider.SubscriberResolves.ContainsKey(type)) return;
            _domainEventHandlerProvider.SubscriberResolves[type].ForEach(handleMethodType =>
            {
                var handler = (IDomainEventHandler) _serviceProvider.GetService(handleMethodType.DeclaringType);
                handleMethodType.Invoke(handler, new object[] {domainEvent});
            });
        }

        public Task PublishAsync<T>(T @event) where T : IDomainEvent
        {
            throw new NotImplementedException();
        }
    }
}
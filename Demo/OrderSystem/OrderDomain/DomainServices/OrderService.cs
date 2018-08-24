using System.Collections.Generic;
using OrderDomain.DomainEvents;
using OrderDomain.IRepository;
using Zaaby.DDD.Abstractions.Domain;
using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace OrderDomain.DomainServices
{
    public class OrderService : IDomainService
    {
        private readonly IOrderRepository _orderRepository;
        //private readonly ICurrencyRepository _currencyRepository;
        private readonly IDomainEventPublisher _domainEventPublisher;

        public OrderService(IOrderRepository orderRepository, 
            //ICurrencyRepository currencyRepository,
            IDomainEventPublisher domainEventPublisher)
        {
            _orderRepository = orderRepository;
            //_currencyRepository = currencyRepository;
            _domainEventPublisher = domainEventPublisher;
        }

        public void ModifyOrderCurrency(List<string> orderIds, string currencyType)
        {
            //var currency = _currencyRepository.Get(currencyType);
            //if (currency == null)
            //    throw new ArgumentException("The curency is not exist.");
            //if (!currency.IsValid)
            //    throw new ArgumentException("The curency is not valid.");
            
            //var orders = _orderRepository.Get(orderIds);
            //orders.ForEach(order => order.ModifyCurrency(currency.Id));
            //_orderRepository.Modify(orders);
        }

        public void PublishDomainEventTest()
        {
            _domainEventPublisher.PublishEvent(new ReceivedOrderEvent());
        }
    }
}
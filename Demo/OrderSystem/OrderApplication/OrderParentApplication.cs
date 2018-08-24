using System;
using System.Linq;
using IFinanceApplication;
using IOrderApplication;
using IOrderApplication.DTOs;
using IShippingApplication;
using IShippingApplication.IntegrationEvents;
using OrderDomain.DomainServices;
using OrderDomain.IRepository;
using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace OrderApplication
{
    public class OrderParentApplication : IOrderParentApplication
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerFinanceApplication _customerFinanceApplication;
        private readonly IFreightApplication _freightApplication;
        private readonly IIntegrationEventBus _eventBus;
        private readonly OrderService _orderService;

        public OrderParentApplication(IOrderRepository orderParentRepository,IIntegrationEventBus eventBus,
            ICustomerFinanceApplication customerFinanceApplication, IFreightApplication freightApplication,
            OrderService orderService)
        {
            _orderRepository = orderParentRepository;
            _customerFinanceApplication = customerFinanceApplication;
            _freightApplication = freightApplication;
            _eventBus = eventBus;
            _orderService = orderService;
        }

        public OrderParentDto Test()
        {
            var i = _customerFinanceApplication.GetCustomer(Guid.NewGuid());
            return new OrderParentDto
            {
                Id = "testId",
                CreateTime = DateTimeOffset.Now
            };
        }

        public int PublishEvent(int quantity)
        {
            quantity = quantity == 0 ? 1000000 : quantity;
            Enumerable.Range(0, quantity).AsParallel().ForAll(p => _eventBus.PublishEvent(new WarehouseOutEvent()));
            return quantity;
        }

        public void DomainEventTest()
        {
            _orderService.PublishDomainEventTest();
            _orderService.PublishDomainEventTest();
        }

        public OrderParentDto GetOrderParentDto(string id)
        {
            var order = _orderRepository.Get(id);
            return new OrderParentDto
            {
                Id = order.Id,
                CreateTime = order.CreateTime
            };
        }

        public string OrderSystemTest()
        {
            return $"{_customerFinanceApplication.FinanceSystemTest()}\r\n" +
                   $"From OrderParentApplication. {DateTimeOffset.Now.UtcTicks}";
        }
    }
}
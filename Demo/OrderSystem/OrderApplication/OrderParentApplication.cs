using System;
using System.Linq;
using IFinanceApplication;
using IOrderApplication;
using IOrderApplication.DTOs;
using IShippingApplication;
using IShippingApplication.IntegrationEvents;
using OrderDomain.IRepository;
using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace OrderApplication
{
    public class OrderParentApplication : IOrderParentApplication
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerFinanceApplication _customerFinanceApplication;
        private readonly IFreightApplication _freightApplication;
        private readonly IEventBus _eventBus;

        public OrderParentApplication(IOrderRepository orderParentRepository,IEventBus eventBus,
            ICustomerFinanceApplication customerFinanceApplication, IFreightApplication freightApplication)
        {
            _orderRepository = orderParentRepository;
            _customerFinanceApplication = customerFinanceApplication;
            _freightApplication = freightApplication;
            _eventBus = eventBus;
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
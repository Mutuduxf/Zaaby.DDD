using Zaaby.DDD.Abstractions.Application;

namespace IShippingApplication.IntegrationEvents
{
    public class WarehouseOutEvent : IIntegrationEvent
    {
        public string ShippingOrderId { get; set; }
    }
}
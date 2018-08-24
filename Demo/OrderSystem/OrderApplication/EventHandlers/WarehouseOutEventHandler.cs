using IShippingApplication.IntegrationEvents;
using Zaaby.DDD.Abstractions.Application;

namespace OrderApplication.EventHandlers
{
    public class WarehouseOutEventHandler : IIntegrationEventHandler<WarehouseOutEvent>
    {
        public void Handle(WarehouseOutEvent integrationEvent)
        {
            var i = this.GetHashCode();
        }
    }
}
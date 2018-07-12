using System;
using Zaaby.DDD.Abstractions.Domain;

namespace OrderDomain.DomainEvents
{
    public class ReceivedOrderEvent : IDomainEvent
    {
        public Guid Id { get; }
        public string OrderId { get; set; }
        public int ChargeLengthByMillimeter { get; set; }
        public int ChargeWidthByMillimeter { get; set; }
        public int ChargeHeightByMillimeter { get; set; }
        public int ChargeWeightByGram { get; set; }
    }
}
using System;
using Zaaby.DDD.Abstractions.Domain;

namespace AppleDomain.DomainEvents
{
    public class AppleDomainEventB : IDomainEvent
    {
        public DateTimeOffset CreateTime { get; set; }
    }
}
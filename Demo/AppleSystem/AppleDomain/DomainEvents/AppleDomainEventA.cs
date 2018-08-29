using System;
using Zaaby.DDD.Abstractions.Domain;

namespace AppleDomain.DomainEvents
{
    public class AppleDomainEventA : IDomainEvent
    {
        public DateTimeOffset CreateTime { get; set; }
    }
}
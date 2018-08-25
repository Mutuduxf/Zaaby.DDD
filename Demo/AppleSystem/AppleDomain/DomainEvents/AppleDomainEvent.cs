using System;
using Zaaby.DDD.Abstractions.Domain;

namespace AppleDomain.DomainEvents
{
    public class AppleDomainEvent : IDomainEvent
    {
        public DateTimeOffset CreateTime { get; set; }
    }
}
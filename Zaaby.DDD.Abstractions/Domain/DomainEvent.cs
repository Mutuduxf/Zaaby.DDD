using System;

namespace Zaaby.DDD.Abstractions.Domain
{
    public class DomainEvent : IDomainEvent
    {
        public Guid Id { get; protected set; }
        public DateTimeOffset Timestamp { get; protected set; }

        public DomainEvent()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTimeOffset.Now;
        }
    }
}
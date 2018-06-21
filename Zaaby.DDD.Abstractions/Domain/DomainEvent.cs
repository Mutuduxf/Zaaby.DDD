using System;

namespace Zaaby.DDD.Abstractions.Domain
{
    public class DomainEvent : IDomainEvent
    {
        public Guid Id { get; protected set; }
        public string Version { get; set; }

        public DomainEvent()
        {
            Id = Guid.NewGuid();
        }
    }
}
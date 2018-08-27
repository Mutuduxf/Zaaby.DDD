using System;
using AppleDomain.DomainEvents;
using Zaaby.DDD.Abstractions.Domain;

namespace AppleDomain.DomainServices.EventHandlers
{
    public class TestDomainEventHandle : IDomainEventHandler<AppleDomainEvent>
    {
        public Guid Id;

        public TestDomainEventHandle()
        {
            Id = Guid.NewGuid();
        }

        public void Handle(AppleDomainEvent domainEvent)
        {
            var i = GetHashCode();
        }
    }
}
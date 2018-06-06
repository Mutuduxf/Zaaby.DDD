using System;
using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace Zaaby.DDD.Abstractions.Domain
{
    public interface IDomainEvent : IEvent, IEntity<Guid>
    {
    }
}
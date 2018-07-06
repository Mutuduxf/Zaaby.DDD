using System;

namespace Zaaby.DDD.Abstractions.Domain
{
    public abstract class AggregateRoot<TId> : IAggregateRoot<TId>
    {
        public TId Id { get; }
        public abstract void Handle(IDomainEvent domainEvent);
    }

    public abstract class AggregateRootWithStringKey : AggregateRoot<string>
    {

    }

    public abstract class AggregateRootWithGuidKey : AggregateRoot<Guid>
    {

    }

    public abstract class AggregateRootWithIntKey : AggregateRoot<int>
    {

    }

    public abstract class AggregateRootWithLongKey : AggregateRoot<long>
    {

    }
}
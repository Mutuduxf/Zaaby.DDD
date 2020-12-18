using System;
using System.Collections.Generic;

namespace Zaaby.DDD.Abstractions.Domain
{
    public class AggregateRoot : IAggregateRoot
    {
        public IList<IDomainEvent> DomainEvents { get; protected set; }
    }

    public class AggregateRoot<TId> : IAggregateRoot<TId>
    {
        public IList<IDomainEvent> DomainEvents { get; }
        public TId Id { get; }
    }

    /// <summary>
    /// Marker aggregate roots with <see cref="T:System.String" /> keys
    /// </summary>
    public class AggregateRootWithStringKey : IAggregateRootWithStringKey
    {
        public IList<IDomainEvent> DomainEvents { get; }
        public string Id { get; }
    }

    /// <summary>
    /// Marker aggregate roots with <see cref="T:System.Guid" /> keys
    /// </summary>
    public class AggregateRootWithGuidKey : IAggregateRootWithGuidKey
    {
        public IList<IDomainEvent> DomainEvents { get; }
        public Guid Id { get; }
    }

    /// <summary>
    /// Marker aggregate roots with <see cref="T:System.Int32" /> keys
    /// </summary>
    public class AggregateRootWithIntKey : IAggregateRootWithIntKey
    {
        public IList<IDomainEvent> DomainEvents { get; }
        public int Id { get; }
    }

    /// <summary>
    /// Marker aggregate roots with <see cref="T:System.Int64" /> keys
    /// </summary>
    public class AggregateRootWithLongKey : IAggregateRootWithLongKey
    {
        public IList<IDomainEvent> DomainEvents { get; }
        public long Id { get; }
    }
}
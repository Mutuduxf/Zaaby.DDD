using System;
using System.Collections.Generic;

namespace Zaaby.DDD.Abstractions.Domain
{
    public interface IAggregateRoot : IEntity
    {
        IList<IDomainEvent> DomainEvents { get; }
    }

    public interface IAggregateRoot<out TId> : IAggregateRoot, IEntity<TId>
    {
    }

    /// <summary>
    /// Marker interface for aggregate roots with <see cref="T:System.String" /> keys
    /// </summary>
    public interface IAggregateRootWithStringKey : IAggregateRoot<string>
    {
    }

    /// <summary>
    /// Marker interface for aggregate roots with <see cref="T:System.Guid" /> keys
    /// </summary>
    public interface IAggregateRootWithGuidKey : IAggregateRoot<Guid>
    {
    }

    /// <summary>
    /// Marker interface for aggregate roots with <see cref="T:System.Int32" /> keys
    /// </summary>
    public interface IAggregateRootWithIntKey : IAggregateRoot<int>
    {
    }

    /// <summary>
    /// Marker interface for aggregate roots with <see cref="T:System.Int64" /> keys
    /// </summary>
    public interface IAggregateRootWithLongKey : IAggregateRoot<long>
    {
    }
}
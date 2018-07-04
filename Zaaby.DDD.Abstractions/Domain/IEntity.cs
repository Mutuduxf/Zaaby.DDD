using System;

namespace Zaaby.DDD.Abstractions.Domain
{
    public interface IEntity
    {
    }

    public interface IEntity<out TId> : IEntity
    {
        TId Id { get; }
    }

    /// <summary>
    /// Marker interface for entities with <see cref="T:System.String" /> keys
    /// </summary>
    public interface IEntityWithStringKey : IEntity<string>
    {
    }

    /// <summary>
    /// Marker interface for entities with <see cref="T:System.Guid" /> keys
    /// </summary>
    public interface IEntityWithGuidKey : IEntity<Guid>
    {
    }

    /// <summary>
    /// Marker interface for entities with <see cref="T:System.Int32" /> keys
    /// </summary>
    public interface IEntityWithIntKey : IEntity<int>
    {
    }

    /// <summary>
    /// Marker interface for entities with <see cref="T:System.Int64" /> keys
    /// </summary>
    public interface IEntityWithLongKey : IEntity<long>
    {
    }
}
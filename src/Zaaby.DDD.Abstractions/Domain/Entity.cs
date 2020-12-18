using System;

namespace Zaaby.DDD.Abstractions.Domain
{
    public class Entity : IEntity
    {
    }

    public class Entity<TId> : IEntity<TId>
    {
        public TId Id { get; }
    }

    /// <summary>
    /// Marker class for entities with <see cref="T:System.String" /> keys
    /// </summary>
    public class EntityWithStringKey : IEntity<string>
    {
        public string Id { get; }
    }

    /// <summary>
    /// Marker class for entities with <see cref="T:System.Guid" /> keys
    /// </summary>
    public class EntityWithGuidKey : IEntity<Guid>
    {
        public Guid Id { get; }
    }

    /// <summary>
    /// Marker class for entities with <see cref="T:System.Int32" /> keys
    /// </summary>
    public class EntityWithIntKey : IEntity<int>
    {
        public int Id { get; }
    }

    /// <summary>
    /// Marker class for entities with <see cref="T:System.Int64" /> keys
    /// </summary>
    public class EntityWithLongKey : IEntity<long>
    {
        public long Id { get; }
    }
}
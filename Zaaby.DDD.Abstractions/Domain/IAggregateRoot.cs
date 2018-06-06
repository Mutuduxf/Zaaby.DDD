namespace Zaaby.DDD.Abstractions.Domain
{
    public interface IAggregateRoot<out TId> : IEntity<TId>
    {

    }
}
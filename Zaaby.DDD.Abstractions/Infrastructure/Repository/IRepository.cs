using Zaaby.DDD.Abstractions.Domain;

namespace Zaaby.DDD.Abstractions.Infrastructure.Repository
{
    public interface IRepository<TAggregateRoot, TId> where TAggregateRoot : IAggregateRoot<TId>
    {
    }
}
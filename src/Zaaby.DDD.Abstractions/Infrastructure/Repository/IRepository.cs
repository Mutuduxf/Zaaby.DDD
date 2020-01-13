using System.Threading.Tasks;
using Zaaby.DDD.Abstractions.Domain;

namespace Zaaby.DDD.Abstractions.Infrastructure.Repository
{
    public interface IRepository
    {
    }

    public interface IRepository<TAggregateRoot> : IRepository where TAggregateRoot : IAggregateRoot
    {
    }

    public interface IRepository<TAggregateRoot, in TId> : IRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot<TId>
    {
        void Add(TAggregateRoot aggregateRoot);
        void Remove(TAggregateRoot aggregateRoot);
        TAggregateRoot Get(TId id);
    }

    public interface IRepositoryAsync<TAggregateRoot, in TId> : IRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot<TId>
    {
        Task AddAsync(TAggregateRoot aggregateRoot);
        Task RemoveAsync(TAggregateRoot aggregateRoot);
        Task<TAggregateRoot> GetAsync(TId id);
    }
}
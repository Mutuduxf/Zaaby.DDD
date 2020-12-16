using System.Threading.Tasks;
using Zaaby.DDD.Abstractions.Domain;

namespace Zaaby.DDD.Abstractions.Infrastructure.Repository
{
    public interface IRepository
    {
    }

    public interface IRepository<in TAggregateRoot> : IRepository where TAggregateRoot : IAggregateRoot
    {
        void Add(TAggregateRoot aggregateRoot);
    }

    public interface IRepositoryAsync<in TAggregateRoot> : IRepository where TAggregateRoot : IAggregateRoot
    {
        Task AddAsync(TAggregateRoot aggregateRoot);
    }

    public interface IRepository<TAggregateRoot, in TId> : IRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot<TId>
    {
        TAggregateRoot Get(TId id);
    }

    public interface IRepositoryAsync<TAggregateRoot, in TId> : IRepositoryAsync<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot<TId>
    {
        Task<TAggregateRoot> GetAsync(TId id);
    }
}
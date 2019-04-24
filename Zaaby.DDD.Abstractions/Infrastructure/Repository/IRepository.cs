using Zaaby.DDD.Abstractions.Domain;

namespace Zaaby.DDD.Abstractions.Infrastructure.Repository
{
    public interface IRepository
    {
    }

    public interface IRepository<TAggregateRoot> : IRepository where TAggregateRoot : IAggregateRoot
    {
    }
}
using Zaaby.DDD.Abstractions.Domain;

namespace Zaaby.DDD.Abstractions.Infrastructure.Repository
{
    public interface IRepository
    {
    }

    public interface IRepository<in TAggregateRoot> : IRepository where TAggregateRoot : IAggregateRoot
    {
    }
}
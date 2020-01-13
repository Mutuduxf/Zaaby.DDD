using AppleDomain.Aggregates;
using Zaaby.DDD.Abstractions.Infrastructure.Repository;

namespace AppleDomain.IRepositories
{
    public interface IAppleRepository : IRepository<Apple>
    {
        int AddRdb(Apple apple);
        void AddMongo(Apple apple);
    }
}
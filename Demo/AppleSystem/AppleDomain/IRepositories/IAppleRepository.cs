using System;
using AppleDomain.AggregateRoots;
using Zaaby.DDD.Abstractions.Infrastructure.Repository;

namespace AppleDomain.IRepositories
{
    public interface IAppleRepository : IRepository<Apple, Guid>
    {

    }
}
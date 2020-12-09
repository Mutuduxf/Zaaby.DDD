using System.Collections.Generic;
using System.Threading.Tasks;
using AppleDomain.Aggregates;
using AppleDomain.IRepositories;
using Zaaby.DDD.Abstractions.Domain;

namespace AppleDomain.DomainServices
{
    public class AppleDomainService : IDomainService
    {
        private readonly IAppleRepository _appleRepository;

        public AppleDomainService(IAppleRepository appleRepository)
        {
            _appleRepository = appleRepository;
        }

        public async Task PublishDomainEventTest()
        {
            var i = _appleRepository.GetHashCode();
        }

        public int AddRdbApple(List<Apple> apples)
        {
            apples.ForEach(apple => _appleRepository.AddRdb(apple));
            return apples.Count;
        }

        public int AddMongoApple(List<Apple> apples)
        {
            apples.ForEach(apple => _appleRepository.AddMongo(apple));
            return apples.Count;
        }
    }
}
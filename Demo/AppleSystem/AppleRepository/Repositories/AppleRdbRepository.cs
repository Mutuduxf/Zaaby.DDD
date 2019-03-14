using System.Data;
using System.Linq;
using AppleDomain.AggregateRoots;
using AppleDomain.IRepositories;
using AppleRepository.POs;
using Dapper;
using Zaabee.Mongo.Abstractions;

namespace AppleRepository.Repositories
{
    public class AppleRdbRepository : IAppleRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDbTransaction _dbTransaction;
        private readonly IZaabeeMongoClient _zaabeeMongoClient;

        public AppleRdbRepository(IDbConnection dbConnection, IDbTransaction dbTransaction,IZaabeeMongoClient zaabeeMongoClient)
        {
            _dbConnection = dbConnection;
            _dbTransaction = dbTransaction;
            _zaabeeMongoClient = zaabeeMongoClient;
        }

        public int AddRdb(Apple apple)
        {
            var applePo = new ApplePo(apple);
            var appleSkinPo = new AppleSkinPo(apple.Skin);
            var appleFleshPo = new AppleFleshPo(apple.Flesh);
            var appleCores = apple.Cores.Select(core => new AppleCorePo(core));
            var total = _dbConnection.Execute("INSERT INTO apple (id) VALUES (@Id)", applePo, _dbTransaction);
            _dbConnection.Execute("INSERT INTO apple_skin (id,color) VALUES (@Id,@Color)", appleSkinPo, _dbTransaction);
            _dbConnection.Execute("INSERT INTO apple_flesh (id,weight_by_gram) VALUES (@Id,@WeightByGram)", appleFleshPo, _dbTransaction);
            _dbConnection.Execute("INSERT INTO apple_core (id) VALUES (@Id)", appleCores, _dbTransaction);
            return total;
        }

        public void AddMongo(Apple apple)
        {
            _zaabeeMongoClient.Add(apple);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AppleDomain.AggregateRoots;
using AppleDomain.IRepositories;
using AppleRepository.POs;
using Dapper;
using Zaabee.Dapper.UnitOfWork.Abstractions;
using Zaabee.Mongo.Abstractions;
using Zaabee.SequentialGuid;

namespace AppleRepository.Repositories
{
    public class AppleRdbRepository : IAppleRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        private readonly IZaabeeMongoClient _zaabeeMongoClient;

        public AppleRdbRepository(IZaabeeDbContext zaabeeDbContext, IZaabeeMongoClient zaabeeMongoClient)
        {
            _connection = zaabeeDbContext.UnitOfWork.Connection;
            _transaction = zaabeeDbContext.UnitOfWork.Transaction;
            _zaabeeMongoClient = zaabeeMongoClient;
        }

        public int AddRdb(Apple apple)
        {
            var (applePo, appleSkinPo, appleFleshPo, appleCores) = ConvertApplePo(apple);
            var total = _connection.Execute("INSERT INTO apple (id) VALUES (@Id)", applePo, _transaction);
            _connection.Execute("INSERT INTO apple_skin (id,color) VALUES (@Id,@Color)", appleSkinPo, _transaction);
            _connection.Execute("INSERT INTO apple_flesh (id,weight_by_gram) VALUES (@Id,@WeightByGram)", appleFleshPo,
                _transaction);
            _connection.Execute("INSERT INTO apple_core (id) VALUES (@Id)", appleCores, _transaction);
            return total;
        }

        public void AddMongo(Apple apple)
        {
            _zaabeeMongoClient.Add(apple);
        }

        private (ApplePo, AppleSkinPo, AppleFleshPo, List<AppleCorePo>) ConvertApplePo(Apple apple)
        {
            var applePo = new ApplePo {Id = apple.Id};
            var appleSkin = new AppleSkinPo
            {
                Id = apple.Skin.Id == Guid.Empty ? SequentialGuidHelper.GenerateComb() : apple.Skin.Id,
                AppleId = apple.Id, Color = apple.Skin.Color
            };
            var appleFlesh = new AppleFleshPo
            {
                Id = apple.Flesh.Id == Guid.Empty ? SequentialGuidHelper.GenerateComb() : apple.Flesh.Id,
                AppleId = apple.Id, WeightByGram = apple.Flesh.WeightByGram
            };
            var appleCores = apple.Cores.Select(core => new AppleCorePo
                    {Id = core.Id == Guid.Empty ? SequentialGuidHelper.GenerateComb() : core.Id, AppleId = apple.Id})
                .ToList();
            return (applePo, appleSkin, appleFlesh, appleCores);
        }
    }
}
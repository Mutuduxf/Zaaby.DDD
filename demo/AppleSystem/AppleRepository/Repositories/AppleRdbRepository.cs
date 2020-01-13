using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AppleDomain.Aggregates;
using AppleDomain.Aggregates.Entities;
using AppleDomain.IRepositories;
using AppleRepository.POs;
using Dapper;
using Zaabee.Mongo.Abstractions;
using Zaabee.SequentialGuid;
using Zaaby.DDD.Abstractions.Infrastructure;

namespace AppleRepository.Repositories
{
    public class AppleRdbRepository : IAppleRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        private readonly IZaabeeMongoClient _zaabeeMongoClient;

        public AppleRdbRepository(IUnitOfWork unitOfWork, IZaabeeMongoClient zaabeeMongoClient)
        {
            _connection = unitOfWork.Connection;
            _transaction = unitOfWork.Transaction;
            _zaabeeMongoClient = zaabeeMongoClient;
        }

        public int AddRdb(Apple apple)
        {
            var (applePo, appleSkinPo, appleFleshPo, appleCores) = ConvertApplePo(apple);
            var total = _connection.Execute("INSERT INTO apple (id) VALUES (@Id)", applePo, _transaction);
            _connection.Execute("INSERT INTO apple_skin (id,apple_id,color) VALUES (@Id,@AppleId,@Color)", appleSkinPo, _transaction);
            _connection.Execute("INSERT INTO apple_flesh (id,apple_id,weight_by_gram) VALUES (@Id,@AppleId,@WeightByGram)", appleFleshPo,
                _transaction);
            _connection.Execute("INSERT INTO apple_core (id,apple_id) VALUES (@Id,@AppleId)", appleCores, _transaction);
            return total;
        }

        public void AddMongo(Apple apple)
        {
            _zaabeeMongoClient.Add(apple);
        }

        public Apple Get(Guid id)
        {
            var applePo =
                _connection.QueryFirstOrDefault<ApplePo>("SELECT * FROM apple WHERE id = @Id", new {Id = id},
                    _transaction);
            if (applePo == null) return null;

            var appleSkinPo =
                _connection.QueryFirstOrDefault<AppleSkinPo>("SELECT * FROM apple_skin WHERE apple_id = @Id",
                    new {AppleId = id}, _transaction);
            var appleFleshPo =
                _connection.QueryFirstOrDefault<AppleFleshPo>("SELECT * FROM apple_flesh WHERE apple_id = @Id",
                    new {AppleId = id}, _transaction);
            var appleCorePos = _connection.Query<AppleCorePo>("SELECT * FROM apple_core WHERE apple_id = @Id",
                new {AppleId = id}, _transaction);

//            if (appleSkinPo != null)
//                var appleSkinState = new AppleSkinState { };
            var appleFleshState = new AppleFleshState { };
            var appleCoreStates = appleCorePos.Select(po => new AppleCoreState { });
            return null;
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
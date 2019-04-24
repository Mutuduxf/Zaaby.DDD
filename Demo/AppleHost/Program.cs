﻿using System.IO;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Zaabee.Dapper.UnitOfWork;
using Zaabee.Dapper.UnitOfWork.Abstractions;
using Zaabee.Mongo;
using Zaabee.Mongo.Abstractions;
using Zaabee.RabbitMQ;
using Zaabee.RabbitMQ.Abstractions;
using Zaabee.RabbitMQ.Jil;
using Zaabee.Redis; 
using Zaabee.Redis.Abstractions;
using Zaaby;
using Zaaby.DDD;
using Zaaby.DDD.Abstractions.Application;
using Zaaby.DDD.EventBus.RabbitMQ;

namespace AppleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("RabbitMQ.json", true, true)
                .AddJsonFile("Redis.json", true, true);
            var config = configBuilder.Build();

            var rabbitMqConfig = config.GetSection("ZaabeeRabbitMQ").Get<MqConfig>();
            var redisConfig = config.GetSection("ZaabeeRedis").Get<RedisConfig>();

            ZaabyServer.GetInstance()
                .UseDDD()
                .UseZaabyServer<IApplicationService>()
                .UseEventBus()
                //RabbitMq
                .AddSingleton<IZaabeeRabbitMqClient>(p =>
                    new ZaabeeRabbitMqClient(rabbitMqConfig, new Serializer()))
                //Redis
                .AddSingleton<IZaabeeRedisClient>(p =>
                    new ZaabeeRedisClient(redisConfig, new Zaabee.Redis.Protobuf.Serializer()))
                //Mongo的客户端以及Query客户端
                .AddSingleton<IZaabeeMongoClient>(p =>
                    new ZaabeeMongoClient("mongodb://TestUser:123@192.168.78.152:27017/TestDB/?readPreference=primary",
                        "TestDB"))
                .AddSingleton<IZaabeeMongoQueryClient>(p =>
                    new ZaabeeMongoClient("mongodb://TestUser:123@192.168.78.152:27017/TestDB/?readPreference=primary",
                        "TestDB"))
                //RDB，均使用IDbConnection注入，两者只选其一
                .AddScoped<IZaabeeDbContext>(p => new ZaabeeDbContext(new NpgsqlConnection(
                    "Host=192.168.78.152;Username=postgres;Password=123qweasd,./;Database=postgres")))
//                .AddScoped<IZaabeeDbContext>(p => new ZaabeeDbContext(new MySqlConnection(
//                    "Database=TestDB;Data Source=192.168.78.152;User Id=root;Password=123qweasd,./;CharSet=utf8;port=3306")))
                .UseUrls("http://*:5001")
                .Run();
        }
    }
}
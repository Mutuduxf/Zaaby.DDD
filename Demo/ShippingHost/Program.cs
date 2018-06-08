using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Zaabee.Mongo;
using Zaabee.Mongo.Abstractions;
using Zaabee.Mongo.Common;
using Zaabee.RabbitMQ;
using Zaabee.Redis;
using Zaaby;
using Zaaby.Client;
using Zaaby.DDD.Abstractions.Application;

namespace ShippingHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("ApplicationService.json", true, true)
                .AddJsonFile("Mongo.json", true, true)
                .AddJsonFile("RabbitMQ.json", true, true)
                .AddJsonFile("Redis.json", true, true);
            var config = configBuilder.Build();

            var appServiceConfig = config.GetSection("ZaabyApplication").Get<Dictionary<string, List<string>>>();
            var mongoConfig = config.GetSection("ZaabeMongo").Get<MongoDbConfiger>();
            var rabbitmqConfig = config.GetSection("ZaabyRabbitMQ").Get<MqConfig>();
            var redisConfig = config.GetSection("ZaabyRedis").Get<RedisConfig>();

            ZaabyServer.GetInstance()
                .UseZaabyClient(appServiceConfig)
                .UseZaabyServer<IApplicationService>()
                .AddSingleton<IZaabeeMongoClient>(p => new ZaabeeMongoClient(mongoConfig))
//                .AddSingleton<IEventBus, ZaabyRabbitMqClient>(p =>
//                    new ZaabyRabbitMqClient(rabbitmqConfig, new Serializer()))
//                .AddSingleton<ICache, ZaabyRedisClient>(p =>
//                    new ZaabyRedisClient(redisConfig, new Zaaby.Cache.RedisProvider.Protobuf.Serializer()))
                .UseUrls("http://*:5002")
                .Run();
        }
    }
}
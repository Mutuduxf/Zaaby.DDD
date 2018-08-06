using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Zaabee.Mongo;
using Zaabee.Mongo.Abstractions;
using Zaabee.Mongo.Common;
using Zaabee.RabbitMQ;
using Zaabee.RabbitMQ.Abstractions;
using Zaabee.RabbitMQ.Jil;
using Zaabee.Redis;
using Zaabee.Redis.Abstractions;
using Zaaby;
using Zaaby.Client;
using Zaaby.DDD;
using Zaaby.DDD.Abstractions.Application;
using Zaaby.DDD.EventBus.RabbitMQ;

namespace OrderHost
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
            var mongoConfig = config.GetSection("ZaabeeMongo").Get<MongoDbConfiger>();
            var rabbitmqConfig = config.GetSection("ZaabeeRabbitMQ").Get<MqConfig>();
            var redisConfig = config.GetSection("ZaabeeRedis").Get<RedisConfig>();

            ZaabyServer.GetInstance()
                .UseZaabyClient(appServiceConfig)
                .UseZaabyServer<IApplicationService>()
                .UseDDD()
                .UseEventBus()
                .AddSingleton<IZaabeeMongoClient>(p => new ZaabeeMongoClient(mongoConfig))
                .AddSingleton<IZaabeeRabbitMqClient>(p =>
                    new ZaabeeRabbitMqClient(rabbitmqConfig, new Serializer()))
                .AddSingleton<IZaabeeRedisClient, ZaabeeRedisClient>(p =>
                    new ZaabeeRedisClient(redisConfig, new Zaabee.Redis.Protobuf.Serializer()))
                .UseUrls("http://*:5001")
                .Run();
        }
    }
}
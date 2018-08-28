using System;

namespace BananaHost
{
    class Program
    {
        static void Main(string[] args)
        {
//            var configBuilder = new ConfigurationBuilder()
//                .SetBasePath(Directory.GetCurrentDirectory())
//                .AddJsonFile("ApplicationService.json", true, true)
//                .AddJsonFile("Mongo.json", true, true)
//                .AddJsonFile("RabbitMQ.json", true, true)
//                .AddJsonFile("Redis.json", true, true);
//            var config = configBuilder.Build();
//
//            var appServiceConfig = config.GetSection("ZaabyApplication").Get<Dictionary<string, List<string>>>();
//            var mongoConfig = config.GetSection("ZaabeeMongo").Get<MongoDbConfiger>();
//            var rabbitMqConfig = config.GetSection("ZaabeeRabbitMQ").Get<MqConfig>();
//            var redisConfig = config.GetSection("ZaabeeRedis").Get<RedisConfig>();
//
//            ZaabyServer.GetInstance()
//                .UseDDD()
//                .UseZaabyClient(appServiceConfig)
//                .UseZaabyServer<IApplicationService>()
//                .UseEventBus()
//                .AddSingleton<IZaabeeMongoClient>(p => new ZaabeeMongoClient(mongoConfig))
//                .AddSingleton<IZaabeeRabbitMqClient>(p =>
//                    new ZaabeeRabbitMqClient(rabbitMqConfig, new Serializer()))
//                .AddSingleton<IZaabeeRedisClient, ZaabeeRedisClient>(p =>
//                    new ZaabeeRedisClient(redisConfig, new Zaabee.Redis.Protobuf.Serializer()))
//                .UseUrls("http://*:5001")
//                .Run();
        }
    }
}
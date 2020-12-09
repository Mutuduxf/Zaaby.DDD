// using System.Linq;
// using Zaaby.Abstractions;
// using Zaaby.DDD.Abstractions.Infrastructure.EventBus;
//
// namespace Zaaby.DDD.EventBus.RabbitMQ
// {
//     public static class ZaabyServerExtension
//     {
//         public static IZaabyServer UseEventBus(this IZaabyServer zaabyServer)
//         {
//             var interfaceType = typeof(IIntegrationEventBus);
//             var eventBusType = LoadHelper.GetAllTypes()
//                 .FirstOrDefault(type => interfaceType.IsAssignableFrom(type) && type.IsClass);
//             if (eventBusType == null) return zaabyServer;
//             zaabyServer.AddSingleton(interfaceType, eventBusType);
//             zaabyServer.RegisterServiceRunner(interfaceType, eventBusType);
//             return zaabyServer;
//         }
//     }
// }
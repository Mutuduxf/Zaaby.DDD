// using System.Linq;
// using Zaaby.Abstractions;
// using Zaaby.DDD.Abstractions.Infrastructure.EventBus;
//
// namespace Zaaby.DDD.EventBus
// {
//     public static class ZaabyServerExtension
//     {
//         public static IZaabyServer UseEventBus(this IZaabyServer zaabyServer, IZaabyMessageHub zaabyMessageHub)
//         {
//             var interfaceType = typeof(IIntegrationEventBus);
//             var eventBusType = LoadHelper.GetAllTypes()
//                 .FirstOrDefault(type => interfaceType.IsAssignableFrom(type) && type.IsClass);
//             if (eventBusType == null) return zaabyServer;
//             zaabyServer.RegisterServiceRunner<IIntegrationEventBus, ZaabyEventBus>();
//             return zaabyServer;
//         }
//     }
// }
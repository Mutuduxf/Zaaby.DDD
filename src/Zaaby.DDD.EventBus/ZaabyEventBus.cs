// using System;
// using System.Threading.Tasks;
// using Zaaby.Abstractions;
// using Zaaby.DDD.Abstractions.Application;
// using Zaaby.DDD.Abstractions.Infrastructure.EventBus;
//
// namespace Zaaby.DDD.EventBus
// {
//     public class ZaabyEventBus : IIntegrationEventBus
//     {
//         private readonly IZaabyMessageHub _messageHub;
//
//         public ZaabyEventBus(IZaabyMessageHub messageHub)
//         {
//             _messageHub = messageHub;
//             _messageHub.RegisterMessageSubscriber(typeof(IIntegrationEventHandler), typeof(IIntegrationEvent),
//                 "Handle");
//         }
//
//         public void Publish<T>(T integrationEvent) where T : IIntegrationEvent =>
//             _messageHub.Publish(integrationEvent);
//
//         public Task PublishAsync<T>(T integrationEvent) where T : IIntegrationEvent =>
//             _messageHub.PublishAsync(integrationEvent);
//
//         public void Subscribe<T>(Func<Action<T>> handle) where T : IIntegrationEvent =>
//             _messageHub.Subscribe(handle);
//     }
// }
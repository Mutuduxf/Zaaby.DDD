using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Zaabee.RabbitMQ.Abstractions;
using Zaaby.DDD.Abstractions.Application;
using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace Zaaby.DDD.EventBus.RabbitMQ
{
    public class ZaabyEventBus : IIntegrationEventBus
    {
        private readonly IZaabeeRabbitMqClient _rabbitMqClient;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly ConcurrentDictionary<Type, string> _queueNameDic =
            new ConcurrentDictionary<Type, string>();

        public ZaabyEventBus(IServiceScopeFactory serviceScopeFactory, IZaabeeRabbitMqClient rabbitMqClient)
        {
            _rabbitMqClient = rabbitMqClient;
            _serviceScopeFactory = serviceScopeFactory;

            RegisterIntegrationEventSubscriber();
        }

        public void PublishEvent<T>(T @event) where T : IIntegrationEvent
        {
            _rabbitMqClient.PublishEvent(GetTypeName(typeof(T)), @event);
        }

        public void SubscribeEvent<T>(Action<T> handle) where T : IIntegrationEvent
        {
            _rabbitMqClient.SubscribeEvent(handle);
        }

        private void RegisterIntegrationEventSubscriber()
        {
            var integrationEventHandlerTypes = ZaabyServerExtension.AllTypes
                .Where(type => type.IsClass && typeof(IIntegrationEventHandler).IsAssignableFrom(type)).ToList();

            var integrationEventTypes = ZaabyServerExtension.AllTypes
                .Where(type => type.IsClass && typeof(IIntegrationEvent).IsAssignableFrom(type)).ToList();

            var rabbitMqClientType = _rabbitMqClient.GetType();
            var subscribeMethod = rabbitMqClientType.GetMethods().First(m =>
                m.Name == "SubscribeEvent" &&
                m.GetParameters()[0].Name == "exchange" &&
                m.GetParameters()[1].Name == "queue" &&
                m.GetParameters()[2].ParameterType.ContainsGenericParameters &&
                m.GetParameters()[2].ParameterType.GetGenericTypeDefinition() == typeof(Action<>));

            integrationEventHandlerTypes.ForEach(integrationEventHandlerType =>
            {
                var handleMethods = integrationEventHandlerType.GetMethods()
                    .Where(m =>
                        m.Name == "Handle" &&
                        m.GetParameters().Count() == 1 &&
                        integrationEventTypes.Contains(m.GetParameters()[0].ParameterType)
                    ).ToList();

                handleMethods.ForEach(handleMethod =>
                {
                    var integrationEventType = handleMethod.GetParameters()[0].ParameterType;

                    var paramTypeName = GetTypeName(integrationEventType);
                    var exchangeName = paramTypeName;
                    var queueName = GetQueueName(handleMethod, paramTypeName);

                    void HandleAction(object integrationEvent)
                    {
                        var actionT = typeof(Action<>).MakeGenericType(integrationEventType);
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var handler = scope.ServiceProvider
                                .GetService(integrationEventHandlerType);
                            var @delegate = Delegate.CreateDelegate(actionT, handler, handleMethod);
                            @delegate.Method.Invoke(handler, new[] {integrationEvent});
                        }
                    }

                    subscribeMethod.MakeGenericMethod(handleMethod.GetParameters()[0].ParameterType)
                        .Invoke(_rabbitMqClient,
                            new object[] {exchangeName, queueName, (Action<object>) HandleAction, (ushort) 10});
                });
            });
        }

        private string GetTypeName(Type type)
        {
            return _queueNameDic.GetOrAdd(type,
                key => !(type.GetCustomAttributes(typeof(MessageVersionAttribute), false).FirstOrDefault() is
                    MessageVersionAttribute msgVerAttr)
                    ? type.ToString()
                    : $"{type.ToString()}[{msgVerAttr.Version}]");
        }

        private string GetQueueName(MemberInfo memberInfo, string eventName)
        {
            return $"{memberInfo.ReflectedType?.FullName}.{memberInfo.Name}[{eventName}]";
        }
    }
}
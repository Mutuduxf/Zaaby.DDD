using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Zaabee.RabbitMQ.Abstractions;
using Zaaby.DDD.Abstractions.Application;
using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace Zaaby.DDD.EventBus.RabbitMQ
{
    public class ZaabyEventBus : IEventBus
    {
        private readonly IZaabeeRabbitMqClient _rabbitMqClient;
        private readonly IServiceProvider _serviceProvider;

        private readonly ConcurrentDictionary<Type, string> _queueNameDic =
            new ConcurrentDictionary<Type, string>();

        public ZaabyEventBus(IServiceProvider serviceProvider, IZaabeeRabbitMqClient rabbitMqClient)
        {
            _rabbitMqClient = rabbitMqClient;
            _serviceProvider = serviceProvider;

            RegisterIntegrationEventSubscriber();
        }

        public void PublishEvent<T>(T @event) where T : IEvent
        {
            _rabbitMqClient.PublishEvent(GetTypeName(typeof(T)), @event);
        }

        public void SubscribeEvent<T>(Action<T> handle) where T : IEvent
        {
            _rabbitMqClient.SubscribeEvent(handle);
        }

        private void RegisterIntegrationEventSubscriber()
        {
            var integrationEventHandlerTypes = ZaabyServerExtension.AllTypes
                .Where(type => type.IsClass && typeof(IIntegrationEventHandler).IsAssignableFrom(type)).ToList();

            var rabbitMqClientType = _rabbitMqClient.GetType();
            var methods = rabbitMqClientType.GetMethods();
            var subscribeMethod = methods.First(m =>
                m.Name == "SubscribeEvent" &&
                m.GetParameters().Count() == 4 &&
                m.GetParameters()[0].Name == "exchange" &&
                m.GetParameters()[1].Name == "queue" &&
                m.GetParameters()[2].ParameterType.ContainsGenericParameters &&
                m.GetParameters()[2].ParameterType.GetGenericTypeDefinition() == typeof(Action<>));

            integrationEventHandlerTypes.ForEach(integrationEventHandlerType =>
            {
                var handleMethod = integrationEventHandlerType.GetMethods()
                    .First(m =>
                        m.Name == "Handle" &&
                        m.GetParameters().Count() == 1 &&
                        typeof(IIntegrationEvent).IsAssignableFrom(m.GetParameters()[0].ParameterType)
                    );

                var paramT = handleMethod.GetParameters()[0].ParameterType;

                var paramTypeName = GetTypeName(paramT);
                var exchangeName = paramTypeName;
                var queueName = GetQueueName(handleMethod, paramTypeName);

                void HandleAction(IIntegrationEvent integrationEvent)
                {
                    var actionT = typeof(Action<>).MakeGenericType(paramT);
                    var handler = _serviceProvider.GetService(integrationEventHandlerType);
                    var @delegate = Delegate.CreateDelegate(actionT, handler, handleMethod);
                    @delegate.Method.Invoke(handler, new object[] {integrationEvent});
                }

                subscribeMethod.MakeGenericMethod(handleMethod.GetParameters()[0].ParameterType)
                    .Invoke(_rabbitMqClient, new object[] {exchangeName, queueName, (Action<IIntegrationEvent>) HandleAction, (ushort) 10});
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
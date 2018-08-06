using System;
using System.Collections.Generic;
using System.Linq;
using Zaaby.Abstractions;
using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace Zaaby.DDD.EventBus.RabbitMQ
{
    public static class ZaabyServerExtension
    {
        internal static List<Type> AllTypes;

        public static IZaabyServer UseEventBus(this IZaabyServer zaabyServer)
        {
            AllTypes = zaabyServer.AllTypes;
            var interfaceType = typeof(IEventBus);
            var eventBusType = AllTypes.FirstOrDefault(type => interfaceType.IsAssignableFrom(type));
            if (eventBusType == null) return zaabyServer;
            zaabyServer.RegisterServiceRunner(interfaceType, eventBusType);
            return zaabyServer;
        }
    }
}
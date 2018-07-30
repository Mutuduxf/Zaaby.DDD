using System;
using System.Collections.Generic;
using System.Linq;
using Zaaby.Abstractions;
using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace Zaaby.DDD.EventBus.RabbitMQ
{
    public static class ZaabyServerExtension
    {
        private static List<Type> _allTypes;

        public static IZaabyServer UseEventBus(this IZaabyServer zaabyServer)
        {
            _allTypes = zaabyServer.AllTypes;
            var interfaceType = typeof(IEventBus);
            var eventBusType = _allTypes.FirstOrDefault(type => interfaceType.IsAssignableFrom(type));
            if (eventBusType == null) return zaabyServer;
            zaabyServer.RegisterServiceRunner(interfaceType, eventBusType);
            return zaabyServer;
        }
    }
}
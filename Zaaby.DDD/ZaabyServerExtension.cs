using System;
using System.Collections.Generic;
using System.Linq;
using Zaaby.Abstractions;
using Zaaby.DDD.Abstractions.Application;
using Zaaby.DDD.Abstractions.Domain;
using Zaaby.DDD.Abstractions.Infrastructure.EventBus;
using Zaaby.DDD.Abstractions.Infrastructure.Repository;

namespace Zaaby.DDD
{
    public static class ZaabyServerExtension
    {
        internal static List<Type> AllTypes;

        public static IZaabyServer UseDDD(this IZaabyServer zaabyServer)
        {
            AllTypes = zaabyServer.AllTypes;
            return zaabyServer.UseApplicationService()
                .UseIntegrationEventHandler()
                .UseDomainService()
                .UseDomainEventHandler()
                .UseRepository();
        }

        public static IZaabyServer UseApplicationService(this IZaabyServer zaabyServer)
        {
            var applicationServiceTypes = AllTypes
                .Where(type => type.IsClass && typeof(IApplicationService).IsAssignableFrom(type)).ToList();
            applicationServiceTypes.ForEach(integrationEventSubscriberType =>
                zaabyServer.AddScoped(integrationEventSubscriberType));
            return zaabyServer;
        }

        public static IZaabyServer UseIntegrationEventHandler(this IZaabyServer zaabyServer)
        {
            var integrationEventSubscriberTypes = AllTypes
                .Where(type => type.IsClass && typeof(IIntegrationEventHandler).IsAssignableFrom(type)).ToList();
            integrationEventSubscriberTypes.ForEach(integrationEventSubscriberType =>
                zaabyServer.AddScoped(integrationEventSubscriberType));
            return zaabyServer;
        }

        public static IZaabyServer UseDomainService(this IZaabyServer zaabyServer)
        {
            var domainServiceTypes = AllTypes
                .Where(type => type.IsClass && typeof(IDomainService).IsAssignableFrom(type)).ToList();
            domainServiceTypes.ForEach(integrationEventSubscriberType =>
                zaabyServer.AddScoped(integrationEventSubscriberType));
            return zaabyServer;
        }

        public static IZaabyServer UseDomainEventHandler(this IZaabyServer zaabyServer)
        {
            var domainEventSubscriberTypes = AllTypes
                .Where(type => type.IsClass && typeof(IDomainEventHandler).IsAssignableFrom(type)).ToList();
            domainEventSubscriberTypes.ForEach(domainEventSubscriberType =>
                zaabyServer.AddScoped(domainEventSubscriberType));
            zaabyServer.RegisterServiceRunner(typeof(IDomainEventPublisher), typeof(DomainEventPublisher));
            return zaabyServer;
        }

        public static IZaabyServer UseRepository(this IZaabyServer zaabyServer)
        {
            var allInterfaces = AllTypes.Where(type => type.IsInterface);

            var repositoryInterfaces = allInterfaces.Where(type => type.GetInterfaces().Any(@interface =>
                @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IRepository<,>)));

            var implementRepositories = AllTypes
                .Where(type => type.IsClass && repositoryInterfaces.Any(i => i.IsAssignableFrom(type)))
                .ToList();

            implementRepositories.ForEach(repository =>
                zaabyServer.AddScoped(repository.GetInterfaces().First(i => repositoryInterfaces.Contains(i)),
                    repository));
            return zaabyServer;
        }
    }
}
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
        private static readonly IList<Type> AllTypes = LoadHelper.GetAllTypes();

        public static IZaabyServer UseDDD(this IZaabyServer zaabyServer)
        {
            return zaabyServer.UseApplicationService()
                .UseIntegrationEventHandler()
                .UseDomainService()
                .UseDomainEventHandler()
                .UseRepository();
        }

        public static IZaabyServer UseApplicationService(this IZaabyServer zaabyServer)
        {
            return UseApplicationService(zaabyServer,
                type => type.IsClass && typeof(IApplicationService).IsAssignableFrom(type));
        }

        public static IZaabyServer UseApplicationService(this IZaabyServer zaabyServer,
            Func<Type, bool> applicationServiceTypeDefinition)
        {
            var applicationServiceTypes = AllTypes
                .Where(applicationServiceTypeDefinition).ToList();
            applicationServiceTypes.ForEach(applicationServiceType =>
                zaabyServer.AddScoped(applicationServiceType));
            return zaabyServer;
        }

        public static IZaabyServer UseIntegrationEventHandler(this IZaabyServer zaabyServer)
        {
            return UseIntegrationEventHandler(zaabyServer,
                type => type.IsClass && typeof(IIntegrationEventHandler).IsAssignableFrom(type));
        }

        public static IZaabyServer UseIntegrationEventHandler(this IZaabyServer zaabyServer,
            Func<Type, bool> integrationEventHandlerTypeDefinition)
        {
            var integrationEventSubscriberTypes = AllTypes
                .Where(integrationEventHandlerTypeDefinition).ToList();
            integrationEventSubscriberTypes.ForEach(integrationEventSubscriberType =>
                zaabyServer.AddScoped(integrationEventSubscriberType));
            return zaabyServer;
        }

        public static IZaabyServer UseDomainService(this IZaabyServer zaabyServer)
        {
            return UseDomainService(zaabyServer, type => type.IsClass && typeof(IDomainService).IsAssignableFrom(type));
        }

        public static IZaabyServer UseDomainService(this IZaabyServer zaabyServer,
            Func<Type, bool> domainServiceTypeDefinition)
        {
            var domainServiceTypes = AllTypes
                .Where(domainServiceTypeDefinition).ToList();
            domainServiceTypes.ForEach(domainServiceType =>
                zaabyServer.AddScoped(domainServiceType));
            return zaabyServer;
        }

        public static IZaabyServer UseDomainEventHandler(this IZaabyServer zaabyServer)
        {
            return UseDomainEventHandler(zaabyServer,
                type => type.IsClass && typeof(IDomainEventHandler).IsAssignableFrom(type));
        }

        public static IZaabyServer UseDomainEventHandler(this IZaabyServer zaabyServer,
            Func<Type, bool> domainEventHandlerDefinition)
        {
            var domainEventSubscriberTypes = AllTypes
                .Where(domainEventHandlerDefinition).ToList();
            domainEventSubscriberTypes.ForEach(domainEventSubscriberType =>
                zaabyServer.AddScoped(domainEventSubscriberType));
            zaabyServer.AddScoped(typeof(IDomainEventPublisher), typeof(DomainEventPublisher));
            zaabyServer.AddSingleton<DomainEventHandlerProvider>();
            return zaabyServer;
        }

        public static IZaabyServer UseRepository(this IZaabyServer zaabyServer)
        {
            return UseRepository(zaabyServer,
                type => type.IsInterface && typeof(IRepository).IsAssignableFrom(type) && type != typeof(IRepository));
        }

        public static IZaabyServer UseRepository(this IZaabyServer zaabyServer,
            Func<Type, bool> repositoryTypeDefinition)
        {
            var repositoryInterfaces = AllTypes.Where(repositoryTypeDefinition);

            var implementRepositories = AllTypes
                .Where(type => type.IsClass && repositoryInterfaces.Any(i => i.IsAssignableFrom(type)))
                .ToList();

            implementRepositories.ForEach(repository =>
                zaabyServer.AddScoped(repository.GetInterfaces().First(i => repositoryInterfaces.Contains(i)),
                    repository));

            var repositories = AllTypes.Where(type => type.IsClass && repositoryTypeDefinition.Invoke(type)).ToList();

            repositories.ForEach(repository => zaabyServer.AddScoped(repository));

            return zaabyServer;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Zaaby.Abstractions;
using Zaaby.DDD.Abstractions.Application;
using Zaaby.DDD.Abstractions.Domain;
// using Zaaby.DDD.Abstractions.Infrastructure.EventBus;
using Zaaby.DDD.Abstractions.Infrastructure.Repository;

namespace Zaaby.DDD
{
    public static class ZaabyServerExtension
    {
        private static readonly IList<Type> AllTypes = LoadHelper.GetAllTypes();

        public static IZaabyServer AddDDD(this IZaabyServer zaabyServer)
        {
            return zaabyServer.AddApplicationService()
                .AddIntegrationEventHandler()
                .AddDomainService()
                .AddDomainEventHandler()
                .AddRepository();
        }

        public static IZaabyServer AddApplicationService(this IZaabyServer zaabyServer)
        {
            var interfaceType = typeof(IApplicationService);
            return AddApplicationService(zaabyServer,
                type => type.IsInterface && interfaceType.IsAssignableFrom(type));
        }

        public static IZaabyServer AddApplicationService(this IZaabyServer zaabyServer,
            Func<Type, bool> applicationServiceTypeDefinition)
        {
            var applicationServiceInterfaces = AllTypes.Where(applicationServiceTypeDefinition);

            var applicationServiceTypes = AllTypes.Where(type =>
                type.IsClass && applicationServiceInterfaces.Any(i => i.IsAssignableFrom(type)));
            
            foreach (var applicationServiceType in applicationServiceTypes)
            {
                var interfaceTypes = applicationServiceType.GetInterfaces()
                    .Where(applicationServiceTypeDefinition);
                foreach (var interfaceType in interfaceTypes)
                    zaabyServer.AddScoped(interfaceType, applicationServiceType);
                zaabyServer.AddScoped(applicationServiceType);
            }
            
            return zaabyServer;
        }

        public static IZaabyServer AddIntegrationEventHandler(this IZaabyServer zaabyServer) =>
            AddIntegrationEventHandler(zaabyServer,
                type => type.IsClass && typeof(IIntegrationEventHandler).IsAssignableFrom(type));

        public static IZaabyServer AddIntegrationEventHandler(this IZaabyServer zaabyServer,
            Func<Type, bool> integrationEventHandlerTypeDefinition)
        {
            var integrationEventSubscriberTypes = AllTypes.Where(integrationEventHandlerTypeDefinition).ToList();
            integrationEventSubscriberTypes.ForEach(integrationEventSubscriberType =>
                zaabyServer.AddScoped(integrationEventSubscriberType));
            return zaabyServer;
        }

        public static IZaabyServer AddDomainService(this IZaabyServer zaabyServer) =>
            AddDomainService(zaabyServer,type => type.IsClass && typeof(IDomainService).IsAssignableFrom(type));

        public static IZaabyServer AddDomainService(this IZaabyServer zaabyServer,
            Func<Type, bool> domainServiceTypeDefinition)
        {
            var domainServiceTypes = AllTypes.Where(domainServiceTypeDefinition).ToList();
            domainServiceTypes.ForEach(domainServiceType => zaabyServer.AddScoped(domainServiceType));
            return zaabyServer;
        }

        public static IZaabyServer AddDomainEventHandler(this IZaabyServer zaabyServer)=>AddDomainEventHandler(zaabyServer,
            type => type.IsClass && typeof(IDomainEventHandler).IsAssignableFrom(type));

        public static IZaabyServer AddDomainEventHandler(this IZaabyServer zaabyServer,
            Func<Type, bool> domainEventHandlerDefinition)
        {
            var domainEventSubscriberTypes = AllTypes.Where(domainEventHandlerDefinition).ToList();
            domainEventSubscriberTypes.ForEach(domainEventSubscriberType =>
                zaabyServer.AddScoped(domainEventSubscriberType));
            // zaabyServer.AddScoped(typeof(IDomainEventPublisher), typeof(DomainEventPublisher));
            zaabyServer.AddSingleton<DomainEventHandlerProvider>();
            return zaabyServer;
        }

        public static IZaabyServer AddRepository(this IZaabyServer zaabyServer)
        {
            var interfaceType = typeof(IRepository);
            return AddRepository(zaabyServer,
                type => type.IsInterface && interfaceType.IsAssignableFrom(type));
        }

        public static IZaabyServer AddRepository(this IZaabyServer zaabyServer,
            Func<Type, bool> repositoryTypeDefinition)
        {
            var repositoryInterfaces = AllTypes.Where(repositoryTypeDefinition);

            var repositoryTypes = AllTypes
                .Where(type => type.IsClass && repositoryInterfaces.Any(i => i.IsAssignableFrom(type)));

            foreach (var repositoryType in repositoryTypes)
            {
                var interfaceTypes = repositoryType.GetInterfaces()
                    .Where(repositoryTypeDefinition);
                foreach (var interfaceType in interfaceTypes)
                    zaabyServer.AddScoped(interfaceType, repositoryType);
                zaabyServer.AddScoped(repositoryType);
            }

            return zaabyServer;
        }
    }
}
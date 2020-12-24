using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Zaaby.Abstractions;
using Zaaby.DDD.Abstractions.Application;
using Zaaby.DDD.Abstractions.Domain;
using Zaaby.DDD.Abstractions.Infrastructure.Repository;

namespace Zaaby.DDD
{
    public static class ZaabyIServiceCollectionExtensions
    {
        private static readonly IList<Type> AllTypes = LoadHelper.GetAllTypes();

        public static IServiceCollection AddDDD(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddApplicationService()
                .AddIntegrationEventHandler()
                .AddDomainService()
                .AddDomainEventHandler()
                .AddRepository();
        }

        public static IServiceCollection AddApplicationService(this IServiceCollection serviceCollection)
        {
            var interfaceType = typeof(IApplicationService);
            return AddApplicationService(serviceCollection,
                type => type.IsInterface && interfaceType.IsAssignableFrom(type));
        }

        public static IServiceCollection AddApplicationService(this IServiceCollection serviceCollection,
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
                    serviceCollection.AddScoped(interfaceType, applicationServiceType);
                serviceCollection.AddScoped(applicationServiceType);
            }

            return serviceCollection;
        }

        public static IServiceCollection AddIntegrationEventHandler(this IServiceCollection serviceCollection) =>
            AddIntegrationEventHandler(serviceCollection,
                type => type.IsClass && typeof(IIntegrationEventHandler).IsAssignableFrom(type));

        public static IServiceCollection AddIntegrationEventHandler(this IServiceCollection serviceCollection,
            Func<Type, bool> integrationEventHandlerTypeDefinition)
        {
            var integrationEventSubscriberTypes = AllTypes.Where(integrationEventHandlerTypeDefinition).ToList();
            integrationEventSubscriberTypes.ForEach(integrationEventSubscriberType =>
                serviceCollection.AddScoped(integrationEventSubscriberType));
            return serviceCollection;
        }

        public static IServiceCollection AddDomainService(this IServiceCollection serviceCollection) =>
            AddDomainService(serviceCollection,type => type.IsClass && typeof(IDomainService).IsAssignableFrom(type));

        public static IServiceCollection AddDomainService(this IServiceCollection serviceCollection,
            Func<Type, bool> domainServiceTypeDefinition)
        {
            var domainServiceTypes = AllTypes.Where(domainServiceTypeDefinition).ToList();
            domainServiceTypes.ForEach(domainServiceType => serviceCollection.AddScoped(domainServiceType));
            return serviceCollection;
        }

        public static IServiceCollection AddDomainEventHandler(this IServiceCollection serviceCollection)=>AddDomainEventHandler(serviceCollection,
            type => type.IsClass && typeof(IDomainEventHandler).IsAssignableFrom(type));

        public static IServiceCollection AddDomainEventHandler(this IServiceCollection serviceCollection,
            Func<Type, bool> domainEventHandlerDefinition)
        {
            var domainEventSubscriberTypes = AllTypes.Where(domainEventHandlerDefinition).ToList();
            domainEventSubscriberTypes.ForEach(domainEventSubscriberType =>
                serviceCollection.AddScoped(domainEventSubscriberType));
            // serviceCollection.AddScoped(typeof(IDomainEventPublisher), typeof(DomainEventPublisher));
            serviceCollection.AddSingleton<DomainEventHandlerProvider>();
            return serviceCollection;
        }

        public static IServiceCollection AddRepository(this IServiceCollection serviceCollection)
        {
            var interfaceType = typeof(IRepository);
            return AddRepository(serviceCollection,
                type => type.IsInterface && interfaceType.IsAssignableFrom(type));
        }

        public static IServiceCollection AddRepository(this IServiceCollection serviceCollection,
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
                    serviceCollection.AddScoped(interfaceType, repositoryType);
                serviceCollection.AddScoped(repositoryType);
            }

            return serviceCollection;
        }
    }
}
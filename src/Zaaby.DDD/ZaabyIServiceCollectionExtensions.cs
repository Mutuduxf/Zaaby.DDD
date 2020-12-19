using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Zaaby.Abstractions;
using Zaaby.DDD.Abstractions.Application;
using Zaaby.DDD.Abstractions.Domain;
// using Zaaby.DDD.Abstractions.Infrastructure.EventBus;
using Zaaby.DDD.Abstractions.Infrastructure.Repository;

namespace Zaaby.DDD
{
    public static class ZaabyIServiceCollectionExtensions
    {
        private static readonly IList<Type> AllTypes = LoadHelper.GetAllTypes();

        public static IServiceCollection UseDDD(this IServiceCollection serviceCollection)
        {
            return serviceCollection.UseApplicationService()
                .UseIntegrationEventHandler()
                .UseDomainService()
                .UseDomainEventHandler()
                .UseRepository();
        }

        public static IServiceCollection UseApplicationService(this IServiceCollection serviceCollection)
        {
            var interfaceType = typeof(IApplicationService);
            return UseApplicationService(serviceCollection,
                type => type.IsInterface && interfaceType.IsAssignableFrom(type));
        }

        public static IServiceCollection UseApplicationService(this IServiceCollection serviceCollection,
            Func<Type, bool> applicationServiceTypeDefinition)
        {
            var applicationServiceInterfaces = AllTypes.Where(applicationServiceTypeDefinition);

            var applicationServiceTypes = AllTypes.Where(type =>
                type.IsClass && applicationServiceInterfaces.Any(i => i.IsAssignableFrom(type))).ToList();

            applicationServiceTypes.ForEach(applicationServiceType =>
                {
                    serviceCollection.AddScoped(applicationServiceType);
                    serviceCollection.AddScoped(
                        applicationServiceType.GetInterfaces().First(i => applicationServiceInterfaces.Contains(i)),
                        applicationServiceType);
                }
            );

            return serviceCollection;
        }

        public static IServiceCollection UseIntegrationEventHandler(this IServiceCollection serviceCollection) =>
            UseIntegrationEventHandler(serviceCollection,
                type => type.IsClass && typeof(IIntegrationEventHandler).IsAssignableFrom(type));

        public static IServiceCollection UseIntegrationEventHandler(this IServiceCollection serviceCollection,
            Func<Type, bool> integrationEventHandlerTypeDefinition)
        {
            var integrationEventSubscriberTypes = AllTypes.Where(integrationEventHandlerTypeDefinition).ToList();
            integrationEventSubscriberTypes.ForEach(integrationEventSubscriberType =>
                serviceCollection.AddScoped(integrationEventSubscriberType));
            return serviceCollection;
        }

        public static IServiceCollection UseDomainService(this IServiceCollection serviceCollection) =>
            UseDomainService(serviceCollection,type => type.IsClass && typeof(IDomainService).IsAssignableFrom(type));

        public static IServiceCollection UseDomainService(this IServiceCollection serviceCollection,
            Func<Type, bool> domainServiceTypeDefinition)
        {
            var domainServiceTypes = AllTypes.Where(domainServiceTypeDefinition).ToList();
            domainServiceTypes.ForEach(domainServiceType => serviceCollection.AddScoped(domainServiceType));
            return serviceCollection;
        }

        public static IServiceCollection UseDomainEventHandler(this IServiceCollection serviceCollection)=>UseDomainEventHandler(serviceCollection,
            type => type.IsClass && typeof(IDomainEventHandler).IsAssignableFrom(type));

        public static IServiceCollection UseDomainEventHandler(this IServiceCollection serviceCollection,
            Func<Type, bool> domainEventHandlerDefinition)
        {
            var domainEventSubscriberTypes = AllTypes.Where(domainEventHandlerDefinition).ToList();
            domainEventSubscriberTypes.ForEach(domainEventSubscriberType =>
                serviceCollection.AddScoped(domainEventSubscriberType));
            // serviceCollection.AddScoped(typeof(IDomainEventPublisher), typeof(DomainEventPublisher));
            serviceCollection.AddSingleton<DomainEventHandlerProvider>();
            return serviceCollection;
        }

        public static IServiceCollection UseRepository(this IServiceCollection serviceCollection)
        {
            var interfaceType = typeof(IRepository);
            return UseRepository(serviceCollection,
                type => type.IsInterface && interfaceType.IsAssignableFrom(type));
        }

        public static IServiceCollection UseRepository(this IServiceCollection serviceCollection,
            Func<Type, bool> repositoryTypeDefinition)
        {
            var repositoryInterfaces = AllTypes.Where(repositoryTypeDefinition);

            var repositoryTypes = AllTypes
                .Where(type => type.IsClass && repositoryInterfaces.Any(i => i.IsAssignableFrom(type)))
                .ToList();

            repositoryTypes.ForEach(repositoryType =>
                {
                    serviceCollection.AddScoped(repositoryType);
                    serviceCollection.AddScoped(
                        repositoryType.GetInterfaces().First(i => repositoryInterfaces.Contains(i)),
                        repositoryType);
                }
            );

            return serviceCollection;
        }
    }
}
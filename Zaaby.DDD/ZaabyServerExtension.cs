using System;
using System.Collections.Generic;
using System.Linq;
using Zaaby.Abstractions;
using Zaaby.DDD.Abstractions.Application;
using Zaaby.DDD.Abstractions.Domain;
using Zaaby.DDD.Abstractions.Infrastructure.Repository;

namespace Zaaby.DDD
{
    public static class ZaabyServerExtension
    {
        private static List<Type> _allTypes;

        public static IZaabyServer UseDDD(this IZaabyServer zaabyServer)
        {
            _allTypes = zaabyServer.AllTypes;
            return zaabyServer.UseApplicationService()
                .UseIntegrationEventHandler()
                .UseDomainService()
                .UseDomainEventHandler()
                .UseRepository();
        }

        public static IZaabyServer UseApplicationService(this IZaabyServer zaabyServer)
        {
            var applicationServiceQuery = _allTypes.Where(type => type.IsClass);
            applicationServiceQuery =
                applicationServiceQuery.Where(type => typeof(IDomainService).IsAssignableFrom(type));

            var applicationServices = applicationServiceQuery.ToList();

            applicationServices.ForEach(applicationService =>
                zaabyServer.AddScoped(applicationService, applicationService));
            return zaabyServer;
        }

        public static IZaabyServer UseIntegrationEventHandler(this IZaabyServer zaabyServer)
        {
            var integrationEventHandlerTypes = _allTypes
                .Where(type => type.IsClass && typeof(IIntegrationEventHandler).IsAssignableFrom(type)).ToList();
            integrationEventHandlerTypes.ForEach(integrationEventHandlerType =>
                zaabyServer.AddScoped(integrationEventHandlerType));
            return zaabyServer;
        }

        public static IZaabyServer UseDomainService(this IZaabyServer zaabyServer)
        {
            var domainServiceQuery = _allTypes.Where(type => type.IsClass);
            domainServiceQuery = domainServiceQuery.Where(type => typeof(IDomainService).IsAssignableFrom(type));

            var domainServices = domainServiceQuery.ToList();

            domainServices.ForEach(domainService => zaabyServer.AddScoped(domainService, domainService));
            return zaabyServer;
        }

        public static IZaabyServer UseDomainEventHandler(this IZaabyServer zaabyServer)
        {
            var domainEventHandlerTypes = _allTypes
                .Where(type => type.IsClass && typeof(IDomainEventHandler).IsAssignableFrom(type)).ToList();
            domainEventHandlerTypes.ForEach(domainEventHandlerType =>
                zaabyServer.AddScoped(domainEventHandlerType));
            return zaabyServer;
        }

        public static IZaabyServer UseRepository(this IZaabyServer zaabyServer)
        {
            var allInterfaces = _allTypes.Where(type => type.IsInterface);

            var repositoryInterfaces = allInterfaces.Where(type => type.GetInterfaces().Any(@interface =>
                @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IRepository<,>)));

            var implementRepositories = _allTypes
                .Where(type => type.IsClass && repositoryInterfaces.Any(i => i.IsAssignableFrom(type)))
                .ToList();

            implementRepositories.ForEach(repository =>
                zaabyServer.AddScoped(repository.GetInterfaces().First(i => repositoryInterfaces.Contains(i)),
                    repository));
            return zaabyServer;
        }
    }
}
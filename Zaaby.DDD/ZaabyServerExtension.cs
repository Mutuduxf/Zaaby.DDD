using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            var integrationEventHandlerQuery = _allTypes.Where(type => type.IsClass);
            integrationEventHandlerQuery = integrationEventHandlerQuery.Where(type =>
                typeof(IIntegrationEventHandler).IsAssignableFrom(type));

            var integrationEventHandlers = integrationEventHandlerQuery.ToList();
            integrationEventHandlers.ForEach(integrationEventHandler =>
            {
                zaabyServer.AddScoped(integrationEventHandler, integrationEventHandler);
                zaabyServer.RegisterServiceRunner(integrationEventHandler);
            });

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
            var domainEventHandlerQuery = _allTypes.Where(type => type.IsClass);
            domainEventHandlerQuery = domainEventHandlerQuery.Where(type =>
                typeof(IDomainEventHandler).IsAssignableFrom(type));

            var domainEventHandlers = domainEventHandlerQuery.ToList();
            domainEventHandlers.ForEach(domainEventHandler =>
            {
                zaabyServer.AddScoped(domainEventHandler, domainEventHandler);
                zaabyServer.RegisterServiceRunner(domainEventHandler);
            });
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

        private static List<Type> GetAllTypes()
        {
            var dir = Directory.GetCurrentDirectory();
            var files = new List<string>();

            files.AddRange(Directory.GetFiles(dir + @"/", "*.dll", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(dir + @"/", "*.exe", SearchOption.AllDirectories));

            var typeDic = new Dictionary<string, Type>();

            foreach (var file in files)
            {
                try
                {
                    foreach (var type in Assembly.LoadFrom(file).GetTypes())
                        if (!typeDic.ContainsKey(type.FullName))
                            typeDic.Add(type.FullName, type);
                }
                catch (BadImageFormatException)
                {
                    // ignored
                }
            }

            return typeDic.Select(kv => kv.Value).ToList();
        }
    }
}
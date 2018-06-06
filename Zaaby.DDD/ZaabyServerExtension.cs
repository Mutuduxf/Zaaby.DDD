using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Zaaby.Core;
using Zaaby.DDD.Abstractions.Application;
using Zaaby.DDD.Abstractions.Domain;
using Zaaby.DDD.Abstractions.Infrastructure.Repository;

namespace Zaaby.DDD
{
    public static class ZaabyServerExtension
    {
        private static readonly List<Type> AllTypes;

        static ZaabyServerExtension()
        {
            AllTypes = GetAllTypes();
        }

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
            var applicationServiceQuery = AllTypes.Where(type => type.IsClass);
            applicationServiceQuery =
                applicationServiceQuery.Where(type => typeof(IDomainService).IsAssignableFrom(type));

            var applicationServices = applicationServiceQuery.ToList();

            applicationServices.ForEach(applicationService =>
                zaabyServer.AddScoped(applicationService, applicationService));
            return zaabyServer;
        }

        public static IZaabyServer UseIntegrationEventHandler(this IZaabyServer zaabyServer)
        {
            var integrationEventHandlerQuery = AllTypes.Where(type => type.IsClass);
            integrationEventHandlerQuery = integrationEventHandlerQuery.Where(type =>
                type.BaseType != null &&
                type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(IntegrationEventHandler<>));

            var integrationEventHandlers = integrationEventHandlerQuery.ToList();
            integrationEventHandlers.ForEach(integrationEventHandler =>
            {
                zaabyServer.AddSingleton(integrationEventHandler, integrationEventHandler);
                zaabyServer.RegisterServiceRunner(integrationEventHandler);
            });

            return zaabyServer;
        }

        public static IZaabyServer UseDomainService(this IZaabyServer zaabyServer)
        {
            var domainServiceQuery = AllTypes.Where(type => type.IsClass);
            domainServiceQuery = domainServiceQuery.Where(type => typeof(IDomainService).IsAssignableFrom(type));

            var domainServices = domainServiceQuery.ToList();

            domainServices.ForEach(domainService => zaabyServer.AddScoped(domainService, domainService));
            return zaabyServer;
        }

        public static IZaabyServer UseDomainEventHandler(this IZaabyServer zaabyServer)
        {
            var domainEventHandlerQuery = AllTypes.Where(type => type.IsClass);
            domainEventHandlerQuery = domainEventHandlerQuery.Where(type =>
                type.BaseType != null &&
                type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(DomainEventHandler<>));

            var domainEventHandlers = domainEventHandlerQuery.ToList();
            domainEventHandlers.ForEach(domainEventHandler =>
            {
                zaabyServer.AddSingleton(domainEventHandler, domainEventHandler);
                zaabyServer.RegisterServiceRunner(domainEventHandler);
            });
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
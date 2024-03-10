using Corely.DataAccess;
using Corely.DataAccess.Connections;
using Corely.DataAccess.Repos;
using Corely.DataAccess.Repos.Accounts;
using Corely.Domain;
using Corely.Domain.Entities.Accounts;
using Corely.Domain.Repos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Corely.UnitTests
{
    public class ServiceFactory : ServiceFactoryBase
    {
        public ServiceFactory() : base() { }

        protected override void AddLogger(IServiceCollection services)
        {
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        }

        protected override void AddDataAccessServices(IServiceCollection services)
        {
            var connection = new DataAccessConnection<string>(ConnectionNames.Mock, "");
            DataServiceFactory.RegisterConnection(connection, services);

            AutoAddMockReposFromAssembly(services, typeof(MockRepoBase<>).Assembly);
            ManualAddReadonlyMockRepos(services);
        }

        private static void AutoAddMockReposFromAssembly(IServiceCollection services, Assembly assembly)
        {
            var repoTypes = assembly.GetTypes().Where(type =>
                type.Namespace != null
                && type.Namespace.StartsWith("Corely.DataAccess.Repos")
                && type.IsClass
                && !type.IsAbstract
                && type.BaseType != null
                && type.BaseType.IsGenericType
                && (type.BaseType.GetGenericTypeDefinition() == typeof(MockRepoBase<>)
                    || type.BaseType.GetGenericTypeDefinition() == typeof(MockRepoExtendedGetBase<>)));

            foreach (var type in repoTypes)
            {
                var interfaceType =
                    // Try to find an interface of IRepoExtendedGet<T> first
                    type.GetInterfaces().FirstOrDefault(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepoExtendedGet<>))
                    ??
                    // If not found, fall back to IRepo<T>
                    type.GetInterfaces().FirstOrDefault(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepo<>));

                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, type);
                }
            }
        }

        private static void ManualAddReadonlyMockRepos(IServiceCollection services)
        {
            services.AddScoped<IReadonlyRepo<AccountEntity>>(serviceProvider =>
            {
                var repo = serviceProvider.GetRequiredService<IRepoExtendedGet<AccountEntity>>();
                return new MockReadonlyAccountRepo((MockAccountRepo)repo);
            });
        }
    }
}

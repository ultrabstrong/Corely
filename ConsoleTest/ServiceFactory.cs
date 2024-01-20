using Corely.DataAccess.Factories;
using Corely.DataAccess.Factories.AccountManagement;
using Corely.Domain;
using Corely.Domain.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ConsoleTest
{
    internal class ServiceFactory : ServiceFactoryBase
    {
        public ServiceFactory() : base() { }

        protected override void AddLogger(IServiceCollection services)
        {
            var logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            services.AddLogging(builder => builder.AddSerilog(logger: logger, dispose: true));
        }

        protected override void AddDataAccessRepos(IServiceCollection services)
        {
            var connection = new DataAccessConnection<string>(
                ConnectionNames.Mock, "");

            // Keyed is used to allow multiple connections to be registered
            services.AddKeyedSingleton<IDataAccessConnection<string>>(
                connection.ConnectionName,
                (serviceProvider, key) => connection);

            // Keyed is to register generic repo factories for each connection
            services.AddKeyedSingleton<IGenericRepoFactory<string>>(
                connection.ConnectionName,
                (serviceProvider, key) => new GenericRepoFactory<string>(
                    serviceProvider.GetRequiredService<ILoggerFactory>(),
                    serviceProvider.GetRequiredKeyedService<IDataAccessConnection<string>>(key)));

            AddAccountManagementDataAccessServices(services);
        }

        private static void AddAccountManagementDataAccessServices(IServiceCollection services)
        {
            // All 'Account management' should belong to one connection type
            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredKeyedService<IGenericRepoFactory<string>>(ConnectionNames.Mock)
                .CreateAccountManagementRepoFactory());

            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredService<IAccountManagementRepoFactory>()
                .CreateAccountRepo());

            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredService<IAccountManagementRepoFactory>()
                .CreateUserRepo());

            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredService<IAccountManagementRepoFactory>()
                .CreateBasicAuthRepo());
        }
    }
}

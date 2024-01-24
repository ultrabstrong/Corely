using Corely.DataAccess.Factories;
using Corely.DataAccess.Factories.AccountManagement;
using Corely.Domain.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess
{
    public class DataServiceFactory<T>
    {
        private readonly IDataAccessConnection<T> _connection;

        public DataServiceFactory(IDataAccessConnection<T> connection)
        {
            _connection = connection;
        }

        public void AddDataServices(IServiceCollection services)
        {
            AddRepoFactories(services);
            AddAccountManagementDataAccessServices(services);
        }

        protected virtual void AddRepoFactories(IServiceCollection services)
        {
            // Keyed is used to allow multiple connections to be registered
            services.AddKeyedSingleton(_connection.ConnectionName,
                (serviceProvider, key) => _connection);

            // Keyed is to register generic repo factories for each connection
            services.AddKeyedSingleton<IGenericRepoFactory<T>>(
                _connection.ConnectionName,
                (serviceProvider, key) => new GenericRepoFactory<T>(
                    serviceProvider.GetRequiredService<ILoggerFactory>(),
                    serviceProvider.GetRequiredKeyedService<IDataAccessConnection<T>>(key)));
        }

        protected virtual void AddAccountManagementDataAccessServices(IServiceCollection services)
        {
            // All 'Account management' should belong to one connection type
            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredKeyedService<IGenericRepoFactory<T>>(_connection.ConnectionName)
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

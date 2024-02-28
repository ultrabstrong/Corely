using Corely.DataAccess.Factories;
using Corely.DataAccess.Factories.AccountManagement;
using Corely.Domain.Services.AccountManagement;
using Microsoft.Extensions.DependencyInjection;

namespace Corely.DataAccess
{
    public class KeyedDataServiceFactory<T>
    {
        private readonly string _connectionKey;

        public KeyedDataServiceFactory(string connectionKey)
        {
            _connectionKey = connectionKey;
        }

        public void AddAllDataServices(IServiceCollection services)
        {
            AddAccountManagementDataAccessServices(services);
        }

        public void AddAccountManagementDataAccessServices(IServiceCollection services)
        {
            // All 'Account management' should belong to one connection type
            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredKeyedService<IGenericRepoFactory<T>>(_connectionKey)
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

            services.AddKeyedScoped(nameof(AccountManagementService), (serviceProvider, _) => serviceProvider
                .GetRequiredService<IAccountManagementRepoFactory>()
                .CreateUnitOfWorkProvider());
        }
    }
}

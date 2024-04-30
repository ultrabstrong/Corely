using Corely.DataAccess.EntityFramework.IAM;
using Corely.DataAccess.Factories;
using Corely.IAM.AccountManagement.Services;
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
                .GetRequiredKeyedService<IGenericRepoFactory>(_connectionKey)
                .CreateAccountManagementRepoFactory());

            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredService<IIAMRepoFactory>()
                .CreateAccountRepo());

            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredService<IIAMRepoFactory>()
                .CreateReadonlyAccountRepo());

            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredService<IIAMRepoFactory>()
                .CreateUserRepo());

            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredService<IIAMRepoFactory>()
                .CreateBasicAuthRepo());

            services.AddKeyedScoped(nameof(IAccountManagementService), (serviceProvider, _) => serviceProvider
                .GetRequiredService<IIAMRepoFactory>()
                .CreateUnitOfWorkProvider());
        }
    }
}

using Corely.DataAccess.EntityFramework.IAM;
using Corely.DataAccess.Factories;
using Corely.IAM.Services;
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
            AddIAMDataAccessServices(services);
        }

        public void AddIAMDataAccessServices(IServiceCollection services)
        {
            // All 'Account management' should belong to one connection type
            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredKeyedService<IGenericRepoFactory>(_connectionKey)
                .CreateIAMRepoFactory());

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
                .CreateReadonlyUserRepo());

            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredService<IIAMRepoFactory>()
                .CreateBasicAuthRepo());

            services.AddKeyedScoped(nameof(IRegistrationService), (serviceProvider, _) => serviceProvider
                .GetRequiredService<IIAMRepoFactory>()
                .CreateUnitOfWorkProvider());
        }
    }
}

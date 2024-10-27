using Corely.DataAccess.Connections;
using Corely.IAM.DataAccess.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.DataAccess
{
    public class DataServiceFactory
    {
        public static void RegisterConnection<T>(IDataAccessConnection<T> connection, IServiceCollection services)
        {
            // Keyed is used to allow multiple connections to be registered
            services.AddKeyedSingleton(connection.ConnectionName,
                (serviceProvider, key) => connection);

            // Keyed is to register generic repo factories for each connection
            services.AddKeyedSingleton<IGenericRepoFactory>(
                connection.ConnectionName,
                (serviceProvider, key) => new GenericRepoFactory<T>(
                    serviceProvider.GetRequiredService<ILoggerFactory>(),
                    serviceProvider.GetRequiredKeyedService<IDataAccessConnection<T>>(key)));

            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredKeyedService<IGenericRepoFactory>(connection.ConnectionName)
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

            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredService<IIAMRepoFactory>()
                .CreateUnitOfWorkProvider());
        }
    }
}

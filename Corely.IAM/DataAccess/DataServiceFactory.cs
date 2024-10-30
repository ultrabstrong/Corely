using Corely.DataAccess.Connections;
using Corely.DataAccess.EntityFramework;
using Corely.IAM.DataAccess.EntityFramework;
using Corely.IAM.DataAccess.Mock;
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

            services.AddScoped<IIAMRepoFactory>(serviceProvider =>
                {
                    return connection.ConnectionName switch
                    {
                        ConnectionNames.EntityFramework =>
                            new EFIAMRepoFactory(
                                serviceProvider.GetRequiredService<ILoggerFactory>(),
                                ((IDataAccessConnection<EFConnection>)connection).GetConnection()),
                        ConnectionNames.Mock =>
                            new MockIAMRepoFactory(),
                        _ =>
                            throw new ArgumentOutOfRangeException(connection.ConnectionName),
                    };
                });

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

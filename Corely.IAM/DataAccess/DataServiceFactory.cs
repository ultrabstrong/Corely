using Corely.DataAccess.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.DataAccess
{
    public class DataServiceFactory
    {
        public static KeyedDataServiceFactory<T> RegisterConnection<T>(IDataAccessConnection<T> connection, IServiceCollection services)
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

            return new KeyedDataServiceFactory<T>(connection.ConnectionName);
        }
    }
}

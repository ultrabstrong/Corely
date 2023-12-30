using Corely.Common.Models.Security;
using Corely.Common.Providers.Security.Keys;
using Corely.DataAccess.Factories;
using Corely.Domain;
using Corely.Domain.Connections;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Mappers;
using Corely.Domain.Models.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;

namespace ConsoleTest
{
    internal class Program
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
        private static readonly Logger logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
#pragma warning restore IDE0052 // Remove unread private members

        static void Main()
        {
            try
            {
                using var serviceProvider = CreateServiceProvider();
                var mapper = serviceProvider.GetRequiredService<IMapProvider>();
                mapper.Map<BasicAuthEntity>(new BasicAuth()
                {
                    ModifiedUtc = DateTime.UtcNow,
                    //Password = new HashedValue("password"),
                    Username = "username"
                });
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred");
            }
            logger.Information("Program finished. Press any key to exit.");
            Console.ReadKey();
        }

        private static ServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();

            var key = new AesKeyProvider().CreateKey();
            var keyProvider = new InMemoryKeyStoreProvider(key);
            services.AddSingleton<IKeyStoreProvider>(keyProvider);

            var connection = new DataAccessConnection<string>(
                ConnectionNames.EntityFrameworkMySql,
                "mysql-connection-string");
            services.AddSingleton<IDataAccessConnection<string>>(connection);

            services.AddScoped<IGenericRepoFactory<string>>(serviceProvider =>
            {
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var connection = serviceProvider.GetRequiredService<IDataAccessConnection<string>>();
                return new GenericRepoFactory<string>(loggerFactory, connection);
            });

            services.AddLogging(builder => builder.AddSerilog(logger));
            services.AddDomainServices();

            return services.BuildServiceProvider();
        }
    }
}
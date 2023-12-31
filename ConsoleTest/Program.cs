using Corely.Common.Models.Security;
using Corely.Common.Providers.Security.Encryption;
using Corely.Common.Providers.Security.Factories;
using Corely.Common.Providers.Security.Hashing;
using Corely.Common.Providers.Security.Keys;
using Corely.DataAccess.Factories;
using Corely.Domain.Connections;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Mappers;
using Corely.Domain.Mappers.AutoMapper;
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
                var hashProvider = serviceProvider.GetRequiredService<IHashProvider>();

                var originalBasicAuth = new BasicAuth()
                {
                    ModifiedUtc = DateTime.UtcNow,
                    Password = new HashedValue(hashProvider).Set("password"),
                    Username = "username"
                };

                var mappedBasicAuthEntity = mapper.Map<BasicAuthEntity>(originalBasicAuth);
                var mappedBasicAuth = mapper.Map<BasicAuth>(mappedBasicAuthEntity);
                var isVerified = mappedBasicAuth.Password.Verify("password");
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

            services.AddLogging(builder => builder.AddSerilog(logger));
            services.AddAutoMapper(typeof(IMapProvider).Assembly);
            services.AddScoped<IMapProvider, AutoMapperMapProvider>();

            AddSecurityServices(services);
            AddDataAccessServices(services);

            return services.BuildServiceProvider();
        }

        private static void AddSecurityServices(IServiceCollection services)
        {
            var key = new AesKeyProvider().CreateKey();
            services.AddScoped<IKeyStoreProvider, InMemoryKeyStoreProvider>(_ =>
                new InMemoryKeyStoreProvider(key));

            services.AddScoped<IEncryptionProviderFactory, EncryptionProviderFactory>();
            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredService<IEncryptionProviderFactory>()
                .GetProvider(EncryptionProviderConstants.AES));

            services.AddScoped<IHashProviderFactory, HashProviderFactory>();
            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredService<IHashProviderFactory>()
                .GetProvider(HashProviderConstants.SALTED_SHA256));
        }

        private static void AddDataAccessServices(IServiceCollection services)
        {
            var connection = new DataAccessConnection<string>(
                ConnectionNames.EntityFrameworkMySql,
                "mysql-connection-string");
            services.AddSingleton<IDataAccessConnection<string>>(connection);

            services.AddScoped<IGenericRepoFactory<string>>(serviceProvider =>
                new GenericRepoFactory<string>(
                    serviceProvider.GetRequiredService<ILoggerFactory>(),
                    serviceProvider.GetRequiredService<IDataAccessConnection<string>>()));
        }
    }
}
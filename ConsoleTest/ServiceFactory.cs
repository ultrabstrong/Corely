using Corely.Common.Providers.Security.Encryption;
using Corely.Common.Providers.Security.Factories;
using Corely.Common.Providers.Security.Hashing;
using Corely.Common.Providers.Security.Keys;
using Corely.DataAccess.Factories;
using Corely.Domain.Connections;
using Corely.Domain.Mappers;
using Corely.Domain.Mappers.AutoMapper;
using Corely.Domain.Validators;
using Corely.Domain.Validators.FluentValidators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ConsoleTest
{
    internal class ServiceFactory : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;

        public ServiceFactory()
        {
            var services = new ServiceCollection();

            AddLogger(services);
            AddMapper(services);
            AddValidator(services);
            AddSecurityServices(services);
            AddDataAccessServices(services);

            _serviceProvider = services.BuildServiceProvider();
        }

        private static void AddLogger(IServiceCollection services)
        {
            var logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            services.AddLogging(builder => builder.AddSerilog(logger: logger, dispose: true));
        }

        private static void AddMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(IMapProvider).Assembly);
            services.AddScoped<IMapProvider, AutoMapperMapProvider>();
        }

        private static void AddValidator(IServiceCollection services)
        {
            services.AddScoped<IFluentValidatorFactory, FluentValidatorFactory>();
            services.AddScoped<IValidationProvider, FluentValidationProvider>();

        }

        private static void AddSecurityServices(IServiceCollection services)
        {
            var key = new AesKeyProvider().CreateKey();
            services.AddScoped<IKeyStoreProvider, InMemoryKeyStoreProvider>(_ =>
                new InMemoryKeyStoreProvider(key));

            services.AddScoped<IEncryptionProviderFactory, EncryptionProviderFactory>();
            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredService<IEncryptionProviderFactory>()
                .GetProvider(EncryptionProviderConstants.AES_CODE));

            services.AddScoped<IHashProviderFactory, HashProviderFactory>();
            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredService<IHashProviderFactory>()
                .GetProvider(HashProviderConstants.SALTED_SHA256_CODE));
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

        public T GetRequiredService<T>() where T : notnull
            => _serviceProvider.GetRequiredService<T>();

        public void Dispose() => _serviceProvider?.Dispose();
    }
}

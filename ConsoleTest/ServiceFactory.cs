using Corely.Common.Models;
using Corely.Common.Providers.Security.Encryption;
using Corely.Common.Providers.Security.Factories;
using Corely.Common.Providers.Security.Hashing;
using Corely.Common.Providers.Security.Keys;
using Corely.DataAccess.Factories;
using Corely.DataAccess.Factories.AccountManagement;
using Corely.Domain.Connections;
using Corely.Domain.Mappers;
using Corely.Domain.Mappers.AutoMapper;
using Corely.Domain.Services.Users;
using Corely.Domain.Validators;
using Corely.Domain.Validators.FluentValidators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ConsoleTest
{
    internal class ServiceFactory : DisposeBase
    {
        private readonly ServiceProvider _serviceProvider;

        public ServiceFactory()
        {
            var services = new ServiceCollection();

            AddLogger(services);
            AddMapper(services);
            AddValidator(services);
            AddSecurityServices(services);
            AddDataAccessRepos(services);
            AddDomainServices(services);

            _serviceProvider = services.BuildServiceProvider();
        }

        private static void AddLogger(IServiceCollection services)
        {
            var logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            services.AddLogging(builder => builder.AddSerilog(logger: logger, dispose: true));
        }

        private static void AddMapper(IServiceCollection services)
        {
            services.AddScoped<IMapProvider, AutoMapperMapProvider>();
            services.AddAutoMapper(typeof(IMapProvider).Assembly);
        }

        private static void AddValidator(IServiceCollection services)
        {
            services.AddScoped<IFluentValidatorFactory, FluentValidatorFactory>();
            services.AddScoped<IValidationProvider, FluentValidationProvider>();
            services.AddValidatorsFromAssemblyContaining<FluentValidationProvider>(includeInternalTypes: true);
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

        private static void AddDataAccessRepos(IServiceCollection services)
        {
            var connection = new DataAccessConnection<string>(
                ConnectionNames.Mock, "");

            // Keyed is used to allow multiple connections to be registered
            services.AddKeyedSingleton<IDataAccessConnection<string>>(
                connection.ConnectionName,
                (serviceProvider, key) => connection);

            // Keyed is to register generic repo factories for each connection
            services.AddKeyedSingleton<IGenericRepoFactory<string>>(
                connection.ConnectionName,
                (serviceProvider, key) => new GenericRepoFactory<string>(
                    serviceProvider.GetRequiredService<ILoggerFactory>(),
                    serviceProvider.GetRequiredKeyedService<IDataAccessConnection<string>>(key)));

            AddAccountManagementDataAccessServices(services);
        }

        private static void AddAccountManagementDataAccessServices(IServiceCollection services)
        {
            // All 'Account management' should belong to one connection type
            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredKeyedService<IGenericRepoFactory<string>>(ConnectionNames.Mock)
                .CreateAccountManagementRepoFactory());

            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredService<IAccountManagementRepoFactory>()
                .CreateBasicAuthRepo());

            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredService<IAccountManagementRepoFactory>()
                .CreateUserRepo());
        }

        private static void AddDomainServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }

        public T GetRequiredService<T>() where T : notnull
            => _serviceProvider.GetRequiredService<T>();

        protected override void DisposeManagedResources() => _serviceProvider?.Dispose();
    }
}

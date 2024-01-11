using Corely.Common.Providers.Security.Factories;
using Corely.Common.Providers.Security.Keys;
using Corely.Domain.Mappers;
using Corely.Domain.Mappers.AutoMapper;
using Corely.Domain.Validators;
using Corely.Domain.Validators.FluentValidators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.Fixtures
{
    public class ServiceFactory : IDisposable
    {
        private readonly LoggerFixture _loggerFixture = new();
        private readonly ServiceProvider _serviceProvider;


        public ServiceFactory()
        {
            var services = new ServiceCollection();

            AddMapper(services);
            AddValidator(services);
            AddSecurityServices(services);

            _serviceProvider = services.BuildServiceProvider();
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
            services.AddValidatorsFromAssemblyContaining<FluentValidationProvider>(includeInternalTypes: true);
        }

        private static void AddSecurityServices(IServiceCollection services)
        {
            var key = new AesKeyProvider().CreateKey();
            services.AddScoped<IKeyStoreProvider, InMemoryKeyStoreProvider>(_ =>
                new InMemoryKeyStoreProvider(key));

            services.AddScoped<IEncryptionProviderFactory, EncryptionProviderFactory>();
            services.AddScoped<IHashProviderFactory, HashProviderFactory>();
        }

        public T GetRequiredService<T>() where T : notnull
            => _serviceProvider.GetRequiredService<T>();

        public LoggerFixture GetLoggerFixture() => _loggerFixture;

        public ILogger<T> CreateLogger<T>()
            => _loggerFixture.CreateLogger<T>();

        public void Dispose() => _serviceProvider?.Dispose();
    }
}

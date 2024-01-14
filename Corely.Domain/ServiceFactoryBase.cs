using Corely.Common.Models;
using Corely.Common.Providers.Security.Factories;
using Corely.Common.Providers.Security.Keys;
using Corely.Domain.Mappers;
using Corely.Domain.Mappers.AutoMapper;
using Corely.Domain.Services.Users;
using Corely.Domain.Validators;
using Corely.Domain.Validators.FluentValidators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Corely.Domain
{
    public abstract class ServiceFactoryBase : DisposeBase
    {
        private readonly ServiceProvider _serviceProvider;

        protected ServiceFactoryBase()
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

        protected override void DisposeManagedResources()
            => _serviceProvider?.Dispose();

        private static void AddMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(IMapProvider).Assembly);
            services.AddScoped<IMapProvider, AutoMapperMapProvider>();
        }

        private static void AddValidator(IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<FluentValidationProvider>(includeInternalTypes: true);
            services.AddScoped<IFluentValidatorFactory, FluentValidatorFactory>();
            services.AddScoped<IValidationProvider, FluentValidationProvider>();
        }

        private static void AddSecurityServices(IServiceCollection services)
        {
            var key = new AesKeyProvider().CreateKey();
            services.AddScoped<IKeyStoreProvider, InMemoryKeyStoreProvider>(_ =>
                new InMemoryKeyStoreProvider(key));

            services.AddScoped<IEncryptionProviderFactory, EncryptionProviderFactory>();
            services.AddScoped<IHashProviderFactory, HashProviderFactory>();
        }

        private static void AddDomainServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }

        protected abstract void AddLogger(IServiceCollection services);
        protected abstract void AddDataAccessRepos(IServiceCollection services);

        public T GetRequiredService<T>() where T : notnull
            => _serviceProvider.GetRequiredService<T>();

        public T GetRequiredKeyedService<T>(string key) where T : notnull
            => _serviceProvider.GetRequiredKeyedService<T>(key);

    }
}

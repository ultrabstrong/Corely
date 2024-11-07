using Corely.Common.Models;
using Corely.IAM.Accounts.Services;
using Corely.IAM.Auth.Services;
using Corely.IAM.Mappers;
using Corely.IAM.Mappers.AutoMapper;
using Corely.IAM.Security.Services;
using Corely.IAM.Services;
using Corely.IAM.Users.Services;
using Corely.IAM.Validators;
using Corely.IAM.Validators.FluentValidators;
using Corely.Security.Encryption;
using Corely.Security.Encryption.Factories;
using Corely.Security.Hashing;
using Corely.Security.Hashing.Factories;
using Corely.Security.PasswordValidation.Providers;
using Corely.Security.Signature;
using Corely.Security.Signature.Factories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.IAM
{
    public abstract class ServiceFactoryBase : DisposeBase
    {
        private readonly ServiceProvider _serviceProvider;

        protected ServiceFactoryBase()
        {
            var services = new ServiceCollection();

            services.AddLogging(AddLogging);
            AddMapper(services);
            AddValidator(services);
            AddSecurityServices(services);
            AddDataServices(services);
            AddDomainServices(services);
            AddPasswordValidation(services);
            services.AddScoped(serviceProvider => GetSecurityConfigurationProvider());

            _serviceProvider = services.BuildServiceProvider();
        }

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
            services.AddSingleton<ISymmetricEncryptionProviderFactory, SymmetricEncryptionProviderFactory>(serviceProvider =>
                new SymmetricEncryptionProviderFactory(SymmetricEncryptionConstants.AES_CODE));

            services.AddSingleton<IAsymmetricEncryptionProviderFactory, AsymmetricEncryptionProviderFactory>(serviceProvider =>
                new AsymmetricEncryptionProviderFactory(AsymmetricEncryptionConstants.RSA_CODE));

            services.AddSingleton<IAsymmetricSignatureProviderFactory, AsymmetricSignatureProviderFactory>(serviceProvider =>
                new AsymmetricSignatureProviderFactory(AsymmetricSignatureConstants.ECDSA_SHA256_CODE));

            services.AddSingleton<IHashProviderFactory, HashProviderFactory>(_ =>
                new HashProviderFactory(HashConstants.SALTED_SHA256_CODE));

            services.AddSingleton<ISecurityService, SecurityService>();
        }

        private static void AddDomainServices(IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<ISignInService, SignInService>();
        }

        protected abstract ISecurityConfigurationProvider GetSecurityConfigurationProvider();
        protected abstract void AddLogging(ILoggingBuilder builder);
        protected abstract void AddDataServices(IServiceCollection services);

        protected virtual void AddPasswordValidation(IServiceCollection services)
        {
            services.AddScoped<IPasswordValidationProvider, PasswordValidationProvider>();
        }

        public T GetRequiredService<T>() where T : notnull
            => _serviceProvider.GetRequiredService<T>();

        protected override void DisposeManagedResources()
            => _serviceProvider?.Dispose();

    }
}

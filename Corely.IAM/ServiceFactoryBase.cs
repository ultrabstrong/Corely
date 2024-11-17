using Corely.IAM.Accounts.Services;
using Corely.IAM.BasicAuths.Services;
using Corely.IAM.Groups.Services;
using Corely.IAM.Mappers;
using Corely.IAM.Mappers.AutoMapper;
using Corely.IAM.Security.Models;
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
    public abstract class ServiceFactoryBase
    {
        public void AddIAMServices(IServiceCollection services)
        {
            services.AddLogging(AddLogging);
            AddMapper(services);
            AddValidator(services);
            AddSecurityServices(services);
            AddDataServices(services);
            AddDomainServices(services);
            services.AddScoped(serviceProvider => GetSecurityConfigurationProvider());
            services.AddScoped(serviceProvider => GetPasswordValidation());
            services.Configure<SecurityOptions>(serviceProvider => GetSecurityOptions());
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
            services.AddScoped<IBasicAuthService, BasicAuthService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IDeregistrationService, DeregistrationService>();
            services.AddScoped<ISignInService, SignInService>();
        }

        protected abstract ISecurityConfigurationProvider GetSecurityConfigurationProvider();
        protected abstract void AddLogging(ILoggingBuilder builder);
        protected abstract void AddDataServices(IServiceCollection services);

        protected virtual IPasswordValidationProvider GetPasswordValidation()
        {
            return new PasswordValidationProvider();
        }

        protected virtual SecurityOptions GetSecurityOptions()
        {
            return new SecurityOptions();
        }
    }
}

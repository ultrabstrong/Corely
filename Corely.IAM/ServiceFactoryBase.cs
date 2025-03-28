﻿using Corely.IAM.Accounts.Processors;
using Corely.IAM.BasicAuths.Processors;
using Corely.IAM.Groups.Processors;
using Corely.IAM.Mappers;
using Corely.IAM.Mappers.AutoMapper;
using Corely.IAM.Permissions.Processors;
using Corely.IAM.Roles.Processors;
using Corely.IAM.Security.Models;
using Corely.IAM.Security.Processors;
using Corely.IAM.Services;
using Corely.IAM.Users.Processors;
using Corely.IAM.Validators;
using Corely.IAM.Validators.FluentValidators;
using Corely.Security.Encryption;
using Corely.Security.Encryption.Factories;
using Corely.Security.Hashing;
using Corely.Security.Hashing.Factories;
using Corely.Security.PasswordValidation.Models;
using Corely.Security.PasswordValidation.Providers;
using Corely.Security.Signature;
using Corely.Security.Signature.Factories;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.IAM;

public abstract class ServiceFactoryBase
{
    public const string SECURITY_OPTIONS = "SecurityOptions";
    public const string PASSWORD_VALIDATION_OPTIONS = "PasswordValidationOptions";

    public void AddIAMServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(IMapProvider).Assembly);
        services.AddScoped<IMapProvider, AutoMapperMapProvider>();


        services.AddValidatorsFromAssemblyContaining<FluentValidationProvider>(includeInternalTypes: true);
        services.AddScoped<IFluentValidatorFactory, FluentValidatorFactory>();
        services.AddScoped<IValidationProvider, FluentValidationProvider>();


        services.AddSingleton<ISymmetricEncryptionProviderFactory, SymmetricEncryptionProviderFactory>(serviceProvider =>
            new SymmetricEncryptionProviderFactory(SymmetricEncryptionConstants.AES_CODE));

        services.AddSingleton<IAsymmetricEncryptionProviderFactory, AsymmetricEncryptionProviderFactory>(serviceProvider =>
            new AsymmetricEncryptionProviderFactory(AsymmetricEncryptionConstants.RSA_CODE));

        services.AddSingleton<IAsymmetricSignatureProviderFactory, AsymmetricSignatureProviderFactory>(serviceProvider =>
            new AsymmetricSignatureProviderFactory(AsymmetricSignatureConstants.ECDSA_SHA256_CODE));

        services.AddSingleton<IHashProviderFactory, HashProviderFactory>(_ =>
            new HashProviderFactory(HashConstants.SALTED_SHA256_CODE));

        services.AddSingleton<ISecurityProcessor, SecurityProcessor>();
        services.AddScoped<IPasswordValidationProvider, PasswordValidationProvider>();

        services.AddScoped(serviceProvider => GetSecurityConfigurationProvider());
        services.Configure<SecurityOptions>(configuration.GetSection(SECURITY_OPTIONS));
        services.Configure<PasswordValidationOptions>(configuration.GetSection(PASSWORD_VALIDATION_OPTIONS));


        services.AddScoped<IAccountProcessor, AccountProcessor>();
        services.AddScoped<IUserProcessor, UserProcessor>();
        services.AddScoped<IBasicAuthProcessor, BasicAuthProcessor>();
        services.AddScoped<IGroupProcessor, GroupProcessor>();
        services.AddScoped<IRoleProcessor, RoleProcessor>();
        services.AddScoped<IPermissionProcessor, PermissionProcessor>();

        services.AddScoped<IRegistrationService, RegistrationService>();
        services.AddScoped<IDeregistrationService, DeregistrationService>();
        services.AddScoped<ISignInService, SignInService>();


        services.AddLogging(AddLogging);
        AddDataServices(services);
    }

    protected abstract void AddLogging(ILoggingBuilder builder);
    protected abstract void AddDataServices(IServiceCollection services);
    protected abstract ISecurityConfigurationProvider GetSecurityConfigurationProvider();
}

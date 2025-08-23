using Corely.IAM.Mappers;
using Corely.IAM.Security.Factories;
using Corely.IAM.Validation;
using Corely.Shared.Providers.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.IAM;

public class ServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceFactory()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }

    public T GetRequiredService<T>() where T : notnull
    {
        return _serviceProvider.GetRequiredService<T>();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(builder => builder.AddConsole());
        services.AddSingleton<IMapProvider, CustomMapProvider>();
        services.AddSingleton<IValidationProvider, ValidationProvider>();
        
        // Add encryption provider configuration
        services.AddSingleton<IKeyProvider, Corely.Shared.Providers.Security.AesKeyProvider>();
        services.AddSingleton<IEncryptionProvider>(provider =>
        {
            var keyProvider = provider.GetRequiredService<IKeyProvider>();
            var key = keyProvider.CreateKey(); // In production, this would come from configuration
            return new Corely.Shared.Providers.Security.AESEncryptionProvider(keyProvider, key);
        });
        services.AddSingleton<ISymmetricEncryptionProviderFactory, SymmetricEncryptionProviderFactory>();
    }
}
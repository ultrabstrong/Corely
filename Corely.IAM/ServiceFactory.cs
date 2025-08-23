using Corely.IAM.Mappers;
using Corely.IAM.Validation;
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
    }
}
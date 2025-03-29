using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Corely.IAM.UnitTests;

public class ServiceFactory : MockDbServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceFactory()
    {
        var serviceCollection = new ServiceCollection();
        var configuration = new ConfigurationManager();
        AddIAMServices(serviceCollection, configuration);
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    protected override ISecurityConfigurationProvider GetSecurityConfigurationProvider()
    {
        return new SecurityConfigurationProvider();
    }

    protected override void AddLogging(ILoggingBuilder builder)
    {
        builder.AddProvider(NullLoggerProvider.Instance);
    }

    public T GetRequiredService<T>() where T : notnull
        => _serviceProvider.GetRequiredService<T>();
}

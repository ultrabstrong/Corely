﻿using Corely.Security.KeyStore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Corely.IAM.UnitTests;

public class MockDbServiceFactoryTests : ServiceFactoryGenericTests
{
    private class MockServiceFactory(IServiceCollection serviceCollection, IConfiguration configuration)
        : MockDbServiceFactory(serviceCollection, configuration)
    {
        private class MockSecurityConfigurationProvider : ISecurityConfigurationProvider
        {
            public ISymmetricKeyStoreProvider GetSystemSymmetricKey() => null!;
        }

        protected override ISecurityConfigurationProvider GetSecurityConfigurationProvider()
            => new MockSecurityConfigurationProvider();

        protected override void AddLogging(ILoggingBuilder builder)
            => builder.AddProvider(NullLoggerProvider.Instance);
    }

    private readonly MockServiceFactory _mockServiceFactory = new(ServiceCollection, Configuration);

    protected override ServiceFactoryBase ServiceFactory => _mockServiceFactory;
}

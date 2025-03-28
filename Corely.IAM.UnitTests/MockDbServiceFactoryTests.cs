﻿using Corely.Security.KeyStore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Corely.IAM.UnitTests;

public class MockDbServiceFactoryTests : ServiceFactoryGenericTests
{
    private class MockServiceFactory : MockDbServiceFactory
    {
        private class MockSecurityConfigurationProvider : ISecurityConfigurationProvider
        {
            public ISymmetricKeyStoreProvider GetSystemSymmetricKey() => null!;
        }

        protected override ISecurityConfigurationProvider GetSecurityConfigurationProvider()
        {
            return new MockSecurityConfigurationProvider();
        }

        protected override void AddLogging(ILoggingBuilder builder)
        {
            builder.AddProvider(NullLoggerProvider.Instance);
        }
    }

    private readonly MockServiceFactory _mockServiceFactory = new();

    protected override ServiceFactoryBase ServiceFactory => _mockServiceFactory;
}

﻿using AutoFixture;
using Corely.DataAccess.EntityFramework.Configurations;
using Corely.Security.KeyStore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Corely.IAM.UnitTests;

public class EFServiceFactoryTests : ServiceFactoryGenericTests
{
    private class TestEFConfiguration : EFInMemoryConfigurationBase
    {
        public override void Configure(DbContextOptionsBuilder optionsBuilder)
        {
            var fixture = new Fixture();
            optionsBuilder.UseInMemoryDatabase(fixture.Create<string>());
        }
    }

    private class MockServiceFactory : EFServiceFactory
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

        protected override IEFConfiguration GetEFConfiguration()
        {
            return new TestEFConfiguration();
        }
    }

    private readonly MockServiceFactory _mockServiceFactory = new();

    protected override ServiceFactoryBase ServiceFactory => _mockServiceFactory;
}
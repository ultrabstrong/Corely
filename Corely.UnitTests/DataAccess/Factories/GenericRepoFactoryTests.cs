﻿using AutoFixture;
using Corely.DataAccess.Connections;
using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.EntityFramework.IAM;
using Corely.DataAccess.Factories;
using Corely.DataAccess.Mock;
using Corely.UnitTests.Fixtures;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess.Factories
{
    public class GenericRepoFactoryTests
    {
        private static readonly Fixture _fixture = new();

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WithNullLogger()
        {
            var ex = Record.Exception(() => new GenericRepoFactory<string>(
                null,
                Moq.Mock.Of<IDataAccessConnection<string>>()));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WithNullConnection()
        {
            var ex = Record.Exception(() => new GenericRepoFactory<string>(
                new Mock<ILoggerFactory>().Object,
                null));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void CreateIAMRepoFactory_ReturnsCorrectType_WithMockConnection()
        {
            var dataAccessConnection = new DataAccessConnection<string>(
                ConnectionNames.Mock, _fixture.Create<string>());

            var genericRepoFactory = new GenericRepoFactory<string>(
                new Mock<ILoggerFactory>().Object,
                dataAccessConnection);

            var IAMRepoFactory = genericRepoFactory.CreateIAMRepoFactory();

            Assert.NotNull(IAMRepoFactory);
            Assert.IsType<MockIAMRepoFactory>(IAMRepoFactory);
        }

        [Fact]
        public void CreateIAMRepoFactory_ReturnsCorrectType_WithEFConnection()
        {
            var connection = new EFConfigurationFixture();
            var dataAccessConnection = new DataAccessConnection<EFConnection>(
                ConnectionNames.EntityFramework, new EFConnection(connection));

            var genericRepoFactory = new GenericRepoFactory<EFConnection>(
                new Mock<ILoggerFactory>().Object,
                dataAccessConnection);

            var IAMRepoFactory = genericRepoFactory.CreateIAMRepoFactory();

            Assert.NotNull(IAMRepoFactory);
            Assert.IsType<EFIAMRepoFactory>(IAMRepoFactory);
        }

        [Fact]
        public void CreateIAMRepoFactory_ThrowsArgumentOutOfRangeException_WithInvalidConnectionName()
        {
            var dataAccessConnection = new DataAccessConnection<string>(
                _fixture.Create<string>(), _fixture.Create<string>());

            var genericRepoFactory = new GenericRepoFactory<string>(
                new Mock<ILoggerFactory>().Object,
                dataAccessConnection);

            var ex = Record.Exception(genericRepoFactory.CreateIAMRepoFactory);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentOutOfRangeException>(ex);
        }
    }
}

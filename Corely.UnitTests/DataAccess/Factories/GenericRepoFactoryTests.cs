﻿using AutoFixture;
using Corely.DataAccess.Factories;
using Corely.DataAccess.Factories.AccountManagement;
using Corely.Domain.Connections;
using Corely.UnitTests.Collections;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess.Factories
{
    [Collection(CollectionNames.ServiceFactory)]
    public class GenericRepoFactoryTests
    {
        private static readonly Fixture _fixture = new();

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WithNullLogger()
        {
            var ex = Record.Exception(() => new GenericRepoFactory<string>(
                null,
                Mock.Of<IDataAccessConnection<string>>()));

            Assert.NotNull(ex);
            Assert.True(ex is ArgumentNullException);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WithNullConnection()
        {
            var ex = Record.Exception(() => new GenericRepoFactory<string>(
                new Mock<ILoggerFactory>().Object,
                null));

            Assert.NotNull(ex);
            Assert.True(ex is ArgumentNullException);
        }

        [Theory, MemberData(nameof(CreateAccountManagementRepoFactoryData))]
        public void CreateAccountManagementRepoFactory_ShouldReturnCorrectType(
            string connectionName,
            string connectionString,
            Type expectedType)
        {
            var dataAccessConnection = new DataAccessConnection<string>(
                connectionName, connectionString);

            var genericRepoFactory = new GenericRepoFactory<string>(
                new Mock<ILoggerFactory>().Object,
                dataAccessConnection);

            var accountManagementRepoFactory = genericRepoFactory.CreateAccountManagementRepoFactory();

            Assert.NotNull(accountManagementRepoFactory);
            Assert.IsType(expectedType, accountManagementRepoFactory);
        }

        public static IEnumerable<object[]> CreateAccountManagementRepoFactoryData =>
            [
                [ConnectionNames.EntityFrameworkMySql,
                    _fixture.Create<string>(),
                    typeof(EfMySqlAccountManagementRepoFactory)],
                [ConnectionNames.Mock,
                    _fixture.Create<string>(),
                    typeof(MockAccountManagementRepoFactory)],
            ];
    }
}

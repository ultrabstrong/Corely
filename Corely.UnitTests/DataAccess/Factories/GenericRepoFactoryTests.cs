using AutoFixture;
using Corely.DataAccess.Connections;
using Corely.DataAccess.Factories;
using Corely.DataAccess.Factories.AccountManagement;
using Corely.UnitTests.Collections;
using Corely.UnitTests.Fixtures;
using Microsoft.Extensions.Logging;
using System.Reflection;

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
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WithNullConnection()
        {
            var ex = Record.Exception(() => new GenericRepoFactory<string>(
                new Mock<ILoggerFactory>().Object,
                null));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory]
        [MemberData(nameof(CreateAccountManagementRepoFactoryTestData))]
        public void CreateAccountManagementRepoFactory_ShouldReturnCorrectType(
            string connectionName,
            object connection,
            Type connectionType,
            Type expectedType)
        {
            Type dataAccessConnectionGenericType = typeof(DataAccessConnection<>).MakeGenericType(connectionType);
            object[] constructorArgs = [connectionName, connection];
            var dataAccessConnectionInstance = Activator.CreateInstance(dataAccessConnectionGenericType, constructorArgs);

            Type genericRepoFactoryType = typeof(GenericRepoFactory<>).MakeGenericType(connectionType);
            object[] repoFactoryArgs = [new Mock<ILoggerFactory>().Object, dataAccessConnectionInstance!];
            var genericRepoFactoryInstance = Activator.CreateInstance(genericRepoFactoryType, repoFactoryArgs);

            MethodInfo? createRepoMethod = genericRepoFactoryType.GetMethod("CreateAccountManagementRepoFactory");
            var accountManagementRepoFactory = createRepoMethod?.Invoke(genericRepoFactoryInstance, null);

            // Assertions
            Assert.NotNull(accountManagementRepoFactory);
            Assert.IsType(expectedType, accountManagementRepoFactory);
        }


        public static IEnumerable<object[]> CreateAccountManagementRepoFactoryTestData =>
            [
                [ConnectionNames.EntityFramework,
                    new EFConfigurationFixture(),
                    typeof(IEFConfiguration),
                    typeof(EFAccountManagementRepoFactory)],
                [ConnectionNames.Mock,
                    _fixture.Create<string>(),
                    typeof(string),
                    typeof(MockAccountManagementRepoFactory)],
            ];

        [Fact]
        public void CreateAccountManagementRepoFactory_ShouldThrowArgumentOutOfRangeException_WithInvalidConnectionName()
        {
            var dataAccessConnection = new DataAccessConnection<string>(
                _fixture.Create<string>(), _fixture.Create<string>());

            var genericRepoFactory = new GenericRepoFactory<string>(
                new Mock<ILoggerFactory>().Object,
                dataAccessConnection);

            var ex = Record.Exception(() => genericRepoFactory.CreateAccountManagementRepoFactory());

            Assert.NotNull(ex);
            Assert.IsType<ArgumentOutOfRangeException>(ex);
        }
    }
}

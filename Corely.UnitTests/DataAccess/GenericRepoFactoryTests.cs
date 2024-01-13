using AutoFixture;
using Corely.DataAccess.Factories;
using Corely.DataAccess.Factories.AccountManagement;
using Corely.Domain.Connections;
using Corely.UnitTests.ClassData;
using Corely.UnitTests.Collections;
using Corely.UnitTests.Fixtures;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess
{
    [Collection(nameof(CollectionNames.ServiceFactory))]
    public class GenericRepoFactoryTests
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly GenericRepoFactory<object> _genericRepoFactory;
        private readonly Fixture _fixture = new();

        public GenericRepoFactoryTests(ServiceFactory serviceFactory)
        {
            _loggerFactory = serviceFactory.GetRequiredService<ILoggerFactory>();
            _genericRepoFactory = new(
                _loggerFactory,
                new DataAccessConnection<object>(
                    _fixture.Create<string>(),
                    _fixture.Create<object>()));
        }

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void GenericRepoFactoryConstructor_ThrowsException_WhenConnectionNameIsInvalid(string connectionName)
        {
            var exception = Record.Exception(() => new GenericRepoFactory<object>(
                _loggerFactory,
                new DataAccessConnection<object>(
                    connectionName,
                    _fixture.Create<object>())));

            Assert.NotNull(exception);

            if (connectionName == null)
            {
                Assert.IsType<ArgumentNullException>(exception);
            }
            else
            {
                Assert.IsType<ArgumentException>(exception);
            }
        }

        [Fact]
        public void GenericRepoFactoryConstructor_ThrowsException_WhenConnectionIsNull()
        {
            var exception = Record.Exception(() => new GenericRepoFactory<object>(
                _loggerFactory,
                null));
            Assert.NotNull(exception);
            Assert.IsType<NullReferenceException>(exception);
        }

        [Theory, MemberData(nameof(CreateAccountManagementRepoFactoryTestData))]
        public void CreateAccountManagementRepoFactory_ReturnsCorrectType<T>(string connectionName, T connection, Type expectedType)
        {
            GenericRepoFactory<T> genericRepoFactory = new(
                _loggerFactory,
                new DataAccessConnection<T>(
                    connectionName,
                    connection));

            var actual = genericRepoFactory.CreateAccountManagementRepoFactory();

            Assert.IsType(expectedType, actual);
        }

        public static IEnumerable<object[]> CreateAccountManagementRepoFactoryTestData()
        {
            Fixture fixture = new();

            yield return new object[] {
                ConnectionNames.EntityFrameworkMySql,
                fixture.Create<string>(),
                typeof(EfMySqlAccountManagementRepoFactory) };
        }

        [Fact]
        public void CreateAccountManagementRepoFactory_ThrowsException_WhenConnectionIsUnknown()
        {
            var ex = Record.Exception(() => _genericRepoFactory.CreateAccountManagementRepoFactory());
            Assert.NotNull(ex);
            Assert.IsType<ArgumentOutOfRangeException>(ex);
        }

        [Fact]
        public void CheckKnownConnectionDataTypes_DoesNotThrowException_WhenConnectionIsUnknown()
        {
            var genericRepoFactoryMock = new Mock<GenericRepoFactory<object>>(
                _loggerFactory,
                new DataAccessConnection<object>(
                    _fixture.Create<string>(),
                    _fixture.Create<object>()));

            void act() => genericRepoFactoryMock.Object.CheckKnownConnectionDataTypes();

            genericRepoFactoryMock.Verify(m =>
                m.ThrowForInvalidDataType<object>(),
                Times.Never);

            Assert.Null(Record.Exception(act));
        }

        [Theory, MemberData(nameof(ThrowForInvalidDataTypeTestData))]
        public void ThrowForInvalidDataType_ThrowsException_WhenDataTypesAreInvalid<T1, T2>(bool shouldThrow, T1 _)
        {
            var ex = Record.Exception(() => _genericRepoFactory.ThrowForInvalidDataType<T1>());

            if (shouldThrow)
            {
                Assert.NotNull(ex);
                Assert.IsType<ArgumentException>(ex);
            }
            else
            {
                Assert.Null(ex);
            }
        }

        public static IEnumerable<object[]> ThrowForInvalidDataTypeTestData()
        {
            Fixture fixture = new();

            yield return new object[] {
                false,
                fixture.Create<object>() };

            yield return new object[] {
                true,
                fixture.Create<string>() };
        }
    }
}

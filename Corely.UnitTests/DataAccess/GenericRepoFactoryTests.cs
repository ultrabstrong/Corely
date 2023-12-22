using AutoFixture;
using Corely.DataAccess.Factories;
using Corely.DataAccess.Factories.AccountManagement;
using Corely.Domain.Connections;
using Corely.UnitTests.ClassData;
using Corely.UnitTests.Collections;
using Corely.UnitTests.Fixtures;
using Corely.UnitTests.Common.Providers.Http;
using Serilog;

namespace Corely.UnitTests.DataAccess
{
    [Collection(nameof(CollectionNames.SerilogCollection))]
    public class GenericRepoFactoryTests
    {
        private readonly ILogger _logger;
        private readonly GenericRepoFactory<object> _genericRepoFactory;
        private readonly Fixture _fixture;

        public GenericRepoFactoryTests(TestLoggerFixture loggerFixture)
        {
            _logger = loggerFixture.Logger.ForContext<HttpProxyProviderTests>();
            _fixture = new();

            _genericRepoFactory = new(
                _logger,
                new DataAccessConnection<object>(
                    _fixture.Create<string>(),
                    _fixture.Create<object>()));
        }

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void GenericRepoFactoryConstructor_ThrowsException_WhenConnectionNameIsInvalid(string connectionName)
        {
            void act() => new GenericRepoFactory<object>(
                _logger,
                new DataAccessConnection<object>(
                    connectionName,
                    _fixture.Create<object>()));

            if (connectionName == null)
                Assert.Throws<ArgumentNullException>(act);
            else
                Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void GenericRepoFactoryConstructor_ThrowsException_WhenConnectionIsNull()
        {
            void act() => new GenericRepoFactory<object>(
                _logger,
                null);

            Assert.Throws<NullReferenceException>(act);
        }

        [Theory, MemberData(nameof(CreateAccountManagementRepoFactoryTestData))]
        public void CreateAccountManagementRepoFactory_ReturnsCorrectType<T>(string connectionName, T connection, Type expectedType)
        {
            GenericRepoFactory<T> genericRepoFactory = new(
                _logger,
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
            void act() => _genericRepoFactory.CreateAccountManagementRepoFactory();

            Assert.Throws<ArgumentOutOfRangeException>(act);
        }

        [Fact]
        public void CheckKnownConnectionDataTypes_DoesNotThrowException_WhenConnectionIsUnknown()
        {
            var genericRepoFactoryMock = new Mock<GenericRepoFactory<object>>(
                _logger,
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
            void act() => _genericRepoFactory.ThrowForInvalidDataType<T1>();

            if (shouldThrow)
            {
                Assert.Throws<ArgumentException>(act);
            }
            else
            {
                Assert.Null(Record.Exception(act));
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

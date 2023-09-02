using AutoFixture;
using Corely.DataAccess.Connections;
using Corely.DataAccess.Factories;
using Corely.DataAccess.Factories.AccountManagement;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.DataAccess
{
    public class GenericRepoFactoryTests
    {
        private readonly Mock<GenericRepoFactory> _genericRepoFactoryMock = new() { CallBase = true };
        private readonly Fixture _fixture = new();

        [Theory, MemberData(nameof(CreateAccountManagementRepoFactoryTestData))]
        public void CreateAccountManagementRepoFactory_ReturnsCorrectType<T>(string connectionName, T connectionInfo, Type expectedType)
        {
            var connection = new DataAccessConnection<T>(
                               connectionName,
                               connectionInfo);

            var actual = _genericRepoFactoryMock.Object.CreateAccountManagementRepoFactory(connection);

            _genericRepoFactoryMock.Verify(m =>
                m.CheckKnownConnectionDataTypes<T>(It.IsAny<IDataAccessConnection<T>>()),
                Times.Once);

            Assert.IsType(expectedType, actual);
        }

        [Fact]
        public void CreateAccountManagementRepoFactory_ThrowsException_WhenConnectionIsUnknown()
        {
            var connection = new DataAccessConnection<object>(
                                 _fixture.Create<string>(),
                                 _fixture.Create<object>());

            void act() => _genericRepoFactoryMock.Object.CreateAccountManagementRepoFactory(connection);

            Assert.Throws<ArgumentOutOfRangeException>(act);
        }

        public static IEnumerable<object[]> CreateAccountManagementRepoFactoryTestData()
        {
            Fixture fixture = new();

            yield return new object[] {
                ConnectionName.EntityFramework,
                fixture.Create<string>(),
                typeof(EFAccountManagementRepoFactory) };
        }

        [Fact]
        public void CheckKnownConnectionDataTypes_DoesNotThrowException_WhenConnectionIsUnknown()
        {
            var connection = new DataAccessConnection<object>(
                _fixture.Create<string>(),
                 _fixture.Create<object>());

            void act() => _genericRepoFactoryMock.Object.CheckKnownConnectionDataTypes(connection);

            _genericRepoFactoryMock.Verify(m =>
                m.ThrowForInvalidDataType<object, object>(It.IsAny<IDataAccessConnection<object>>()),
                Times.Never);

            Assert.Null(Record.Exception(act));
        }

        [Fact]
        public void CheckKnownConnectionDataTypes_ThrowsException_WhenConnectionIsNull()
        {
            DataAccessConnection<object>? nullConnection = null;
            void act() => _genericRepoFactoryMock.Object.CheckKnownConnectionDataTypes(nullConnection);
            Assert.Throws<ArgumentNullException>(act);
        }

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void CheckKnownConnectionDataTypes_ThrowsException_WhenConnectionNameIsInvalid(string connectionName)
        {
            var connection = new DataAccessConnection<object>(
                connectionName,
                _fixture.Create<object>());

            void act() => _genericRepoFactoryMock.Object.CheckKnownConnectionDataTypes(connection);

            if (connection.ConnectionName == null)
                Assert.Throws<ArgumentNullException>(act);
            else
                Assert.Throws<ArgumentException>(act);
        }

        [Theory, MemberData(nameof(ThrowForInvalidDataTypeTestData))]
        public void ThrowForInvalidDataType_ThrowsException_WhenDataTypesAreInvalid<T1, T2>(bool shouldThrow, T1 _, IDataAccessConnection<T2> connection)
        {
            void act() => _genericRepoFactoryMock.Object.ThrowForInvalidDataType<T1, T2>(connection);

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

            var connection = new DataAccessConnection<string>(
                               fixture.Create<string>(),
                               fixture.Create<string>());

            yield return new object[] {
                false,
                fixture.Create<string>(),
                connection };
            yield return new object[] {
                true,
                fixture.Create<int>(),
                connection };
        }
    }
}

using AutoFixture;
using Corely.DataAccess.Connections;
using Corely.DataAccess.Factories;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.DataAccess
{
    public class GenericRepoFactoryTests
    {
        private readonly GenericRepoFactory _genericRepoFactory = new();
        private readonly Fixture _fixture = new();
        private static readonly Fixture _memberFixture = new();

        [Fact]
        public void CheckKnownConnectionDataTypes_DoesNotThrowException_WhenConnectionIsUnknown()
        {
            var connection = new DataAccessStringConnection(
                _memberFixture.Create<string>(),
                 _memberFixture.Create<string>());
            void act() => _genericRepoFactory.CheckKnownConnectionDataTypes(connection);
            Assert.Null(Record.Exception(act));
        }

        [Fact]
        public void CheckKnownConnectionDataTypes_ThrowsException_WhenConnectionIsNull()
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            void act() => _genericRepoFactory.CheckKnownConnectionDataTypes((DataAccessStringConnection)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            Assert.Throws<ArgumentNullException>(act);
        }

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void CheckKnownConnectionDataTypes_ThrowsException_WhenConnectionNameIsInvalid(string connectionName)
        {
            var connection = new DataAccessStringConnection(connectionName, "");
            void act() => _genericRepoFactory.CheckKnownConnectionDataTypes(connection);
            if (connection.ConnectionName == null)
                Assert.Throws<ArgumentNullException>(act);
            else
                Assert.Throws<ArgumentException>(act);
        }

        [Theory, MemberData(nameof(ThrowForInvalidDataTypeTestData))]
        public void ThrowForInvalidDataType_ThrowsException_WhenDataTypesAreInvalid<T1, T2>(bool shouldThrow, T1 _, IDataAccessConnection<T2> connection)
        {
            void act() => _genericRepoFactory.ThrowForInvalidDataType<T1, T2>(connection);
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
            var connection = new DataAccessStringConnection(
                               _memberFixture.Create<string>(),
                               _memberFixture.Create<string>());
            yield return new object[] {
                false,
                _memberFixture.Create<string>(),
                connection };
            yield return new object[] {
                true,
                _memberFixture.Create<int>(),
                connection };
        }
    }
}

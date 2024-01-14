using AutoFixture;
using Corely.Domain.Connections;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Domain.Connections
{
    public class DataAccessConnectionTests
    {
        private struct InvalidDataType { };

        private readonly Fixture _fixture = new();

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void Constructor_ShouldThrow_WithNullOrWhiteSpaceConnectionName(string connectionName)
        {
            var ex = Record.Exception(() => new DataAccessConnection<string>(
                connectionName,
                _fixture.Create<string>()));

            Assert.NotNull(ex);
            Assert.True(ex is ArgumentException || ex is ArgumentNullException);
        }

        [Theory]
        [InlineData(ConnectionNames.EntityFrameworkMySql)]
        public void Constructor_ShouldThrowArgumentException_WithInvalidConnectionType(string connectionName)
        {
            var ex = Record.Exception(() => new DataAccessConnection<InvalidDataType>(
                connectionName,
                new InvalidDataType()));

            Assert.NotNull(ex);
            Assert.True(ex is ArgumentException);
        }

        [Fact]
        public void Constructor_ShouldAllowAnyConnectionType_WithMockConnection()
        {
            var ex = Record.Exception(() => new DataAccessConnection<InvalidDataType>(
                ConnectionNames.Mock,
                new InvalidDataType()));

            Assert.Null(ex);
        }

        [Fact]
        public void Constructor_ShouldAllowUnknownConnectionType()
        {
            var ex = Record.Exception(() => new DataAccessConnection<InvalidDataType>(
                _fixture.Create<string>(),
                new InvalidDataType()));

            Assert.Null(ex);
        }
    }
}

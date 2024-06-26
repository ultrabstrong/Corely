﻿using AutoFixture;
using Corely.DataAccess.Connections;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.DataAccess.Connections
{
    public class DataAccessConnectionTests
    {
        private struct InvalidDataType { };

        private readonly Fixture _fixture = new();

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void Constructor_Throws_WithNullOrWhiteSpaceConnectionName(string connectionName)
        {
            var ex = Record.Exception(() => new DataAccessConnection<string>(
                connectionName,
                _fixture.Create<string>()));

            Assert.NotNull(ex);
            Assert.True(ex is ArgumentException || ex is ArgumentNullException);
        }

        [Theory]
        [InlineData(ConnectionNames.EntityFramework)]
        public void Constructor_ThrowsArgumentException_WithInvalidConnectionType(string connectionName)
        {
            var ex = Record.Exception(() => new DataAccessConnection<InvalidDataType>(
                connectionName,
                new InvalidDataType()));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void Constructor_AllowsAnyConnectionType_WithMockConnection()
        {
            var ex = Record.Exception(() => new DataAccessConnection<InvalidDataType>(
                ConnectionNames.Mock,
                new InvalidDataType()));

            Assert.Null(ex);
        }

        [Fact]
        public void Constructor_AllowsUnknownConnectionType()
        {
            var ex = Record.Exception(() => new DataAccessConnection<InvalidDataType>(
                _fixture.Create<string>(),
                new InvalidDataType()));

            Assert.Null(ex);
        }
    }
}

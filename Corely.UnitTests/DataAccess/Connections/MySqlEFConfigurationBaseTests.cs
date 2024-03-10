using AutoFixture;
using Corely.DataAccess.Connections;
using Microsoft.EntityFrameworkCore;

namespace Corely.UnitTests.DataAccess.Connections
{
    public class MySqlEFConfigurationBaseTests : EFConfigurationTestsBase
    {
        private class MockMySqlEFConfiguration : MySqlEFConfigurationBase
        {
            public MockMySqlEFConfiguration(string connectionString) : base(connectionString)
            {
            }

            public override void Configure(DbContextOptionsBuilder optionsBuilder)
            {
            }
        }

        private readonly Fixture _fixture = new();
        private readonly MockMySqlEFConfiguration _mockMySqlEFConfiguration;

        public MySqlEFConfigurationBaseTests()
        {
            _mockMySqlEFConfiguration = new(_fixture.Create<string>());
        }

        [Fact]
        public void GetDbTypes_ReturnsEFDbTypes()
        {
            var dbTypes = _mockMySqlEFConfiguration.GetDbTypes();
            VerifyDbTypes(dbTypes);
        }
    }
}

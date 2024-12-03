using AutoFixture;
using Corely.DataAccess.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Corely.UnitTests.DataAccess.EntityFramework.Configurations;

public class EFMySqlConfigurationBaseTests : EFConfigurationTestsBase
{
    private class MockMySqlEFConfiguration : EFMySqlConfigurationBase
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

    public EFMySqlConfigurationBaseTests()
    {
        _mockMySqlEFConfiguration = new(_fixture.Create<string>());
    }

    protected override IEFConfiguration EFConfiguration => _mockMySqlEFConfiguration;
}

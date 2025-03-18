using AutoFixture;
using Corely.DataAccess.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Corely.DataAccess.UnitTests.EntityFramework.Configurations;

public class EFPostgresConfigurationBaseTests : EFConfigurationTestsBase
{
    private class MockPostgresEFConfiguration : EFPostgresConfigurationBase
    {
        public MockPostgresEFConfiguration(string connectionString) : base(connectionString)
        {
        }
        public override void Configure(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }

    private readonly Fixture _fixture = new();
    private readonly MockPostgresEFConfiguration _mockPostgresEFConfiguration;

    public EFPostgresConfigurationBaseTests()
    {
        _mockPostgresEFConfiguration = new(_fixture.Create<string>());
    }

    protected override IEFConfiguration EFConfiguration => _mockPostgresEFConfiguration;
}

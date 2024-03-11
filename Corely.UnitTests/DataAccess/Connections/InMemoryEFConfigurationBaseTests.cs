using Corely.DataAccess.Connections;
using Microsoft.EntityFrameworkCore;

namespace Corely.UnitTests.DataAccess.Connections
{
    public class InMemoryEFConfigurationBaseTests : EFConfigurationTestsBase
    {
        private class MockInMemoryEFConfiguration : InMemoryEFConfigurationBase
        {
            public override void Configure(DbContextOptionsBuilder optionsBuilder)
            {
            }
        }

        private readonly MockInMemoryEFConfiguration _mockInMemoryEFConfiguration = new();

        protected override IEFConfiguration EFConfiguration => _mockInMemoryEFConfiguration;
    }
}

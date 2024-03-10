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

        [Fact]
        public void GetDbTypes_ReturnsEFDbTypes()
        {
            var dbTypes = _mockInMemoryEFConfiguration.GetDbTypes();
            VerifyDbTypes(dbTypes);
        }
    }
}

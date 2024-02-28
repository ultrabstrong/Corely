using Corely.DataAccess.Connections;
using Microsoft.EntityFrameworkCore;

namespace Corely.UnitTests.Fixtures
{
    public class EFConfigurationFixture : IEFConfiguration
    {
        public void Configure(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("EFAccountRepoTests");
        }
    }
}

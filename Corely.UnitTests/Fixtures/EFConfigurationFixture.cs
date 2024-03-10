using AutoFixture;
using Corely.DataAccess.Connections;
using Microsoft.EntityFrameworkCore;

namespace Corely.UnitTests.Fixtures
{
    public class EFConfigurationFixture : IEFConfiguration
    {
        public void Configure(DbContextOptionsBuilder optionsBuilder)
        {
            var fixture = new Fixture();
            optionsBuilder.UseInMemoryDatabase(fixture.Create<string>());
        }
    }
}

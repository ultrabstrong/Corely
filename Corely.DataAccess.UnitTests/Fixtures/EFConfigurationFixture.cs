using AutoFixture;
using Corely.DataAccess.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Corely.DataAccess.UnitTests.Fixtures;

public class EFConfigurationFixture : EFInMemoryConfigurationBase
{
    public override void Configure(DbContextOptionsBuilder optionsBuilder)
    {
        var fixture = new Fixture();
        optionsBuilder.UseInMemoryDatabase(fixture.Create<string>());
    }
}

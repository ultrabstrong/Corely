using Corely.DataAccess.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Corely.DataAccess.UnitTests.Fixtures;

public class DbContextFixture : DbContext
{
#pragma warning disable IDE0060 // Remove unused parameter
    public DbContextFixture(IEFConfiguration efConfiguration) : base() { }
#pragma warning restore IDE0060 // Remove unused parameter


    public DbContextFixture(DbContextOptions<DbContextFixture> options)
        : base(options)
    {
    }

    public DbSet<EntityFixture> Entities { get; set; } = null!;
}

using Microsoft.EntityFrameworkCore;

namespace Corely.UnitTests.Fixtures;

public class DbContextFixture : DbContext
{
    public DbContextFixture(DbContextOptions<DbContextFixture> options)
        : base(options)
    {
    }

    public DbSet<EntityFixture> Entities { get; set; } = null!;
}

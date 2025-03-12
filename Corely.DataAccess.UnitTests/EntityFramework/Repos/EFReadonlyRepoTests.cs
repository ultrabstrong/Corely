using AutoFixture;
using Corely.DataAccess.EntityFramework.Repos;
using Corely.DataAccess.Interfaces.Repos;
using Corely.DataAccess.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.UnitTests.EntityFramework.Repos;

public class EFReadonlyRepoTests : ReadonlyRepoTestsBase
{
    private readonly DbContext _dbContext;
    private readonly EFReadonlyRepo<EntityFixture> _efReadonlyRepo;

    public EFReadonlyRepoTests()
    {
        _dbContext = GetDbContext();

        _efReadonlyRepo = new(
            Moq.Mock.Of<ILogger<EFRepo<EntityFixture>>>(),
            _dbContext);
    }

    private static DbContextFixture GetDbContext()
    {
        var fixture = new Fixture();
        var options = new DbContextOptionsBuilder<DbContextFixture>()
            .UseInMemoryDatabase(databaseName: fixture.Create<string>())
            .Options;

        var dbContext = new DbContextFixture(options);

        var entityList = fixture.CreateMany<EntityFixture>(5).ToList();
        foreach (var entity in entityList)
        {
            dbContext.Entities.Add(entity);
        }
        dbContext.SaveChanges();

        return dbContext;
    }

    protected override IReadonlyRepo<EntityFixture> ReadonlyRepo
        => _efReadonlyRepo;

    protected override int FillRepoAndReturnId()
        => _dbContext.Set<EntityFixture>().Skip(1).First().Id;
}

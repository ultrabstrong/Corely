using AutoFixture;
using Corely.DataAccess.Interfaces.Repos;
using Corely.DataAccess.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Reflection;

namespace Corely.DataAccess.UnitTests;

public abstract class ReadonlyRepoTestsBase
{
    protected readonly Fixture Fixture = new();

    protected abstract IReadonlyRepo<EntityFixture> ReadonlyRepo { get; }

    protected abstract int FillRepoAndReturnId();

    [Fact]
    public void AllRepoMethodsAreVirtual()
    {
        var readonlyRepoType = ReadonlyRepo.GetType();

        var methods = readonlyRepoType.GetMethods(
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance |
            BindingFlags.DeclaredOnly);

        foreach (var method in methods)
        {
            Assert.True(method.IsVirtual, $"Method {method.Name} is not marked virtual");
        }
    }

    [Fact]
    public async Task GetAsync_ReturnsEntity_WIthIdLookup()
    {
        var id = FillRepoAndReturnId();
        var result = await ReadonlyRepo.GetAsync(id);

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetAsync_ReturnsEntity_WithVerboseIdLookup()
    {
        var id = FillRepoAndReturnId();
        var result = await ReadonlyRepo.GetAsync(u => u.Id == id);

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }


    [Fact]
    public async Task GetAsync_Uses_OrderBy()
    {
        var orderByMock = new Mock<Func<IQueryable<EntityFixture>, IOrderedQueryable<EntityFixture>>>();
        orderByMock
            .Setup(m => m(
                It.IsAny<IQueryable<EntityFixture>>()))
            .Returns((IQueryable<EntityFixture> q) =>
                q.OrderBy(u => u.Id));

        await ReadonlyRepo.GetAsync(
            u => u.Id == 1,
            orderBy: orderByMock.Object);

        orderByMock.Verify(
            m => m(It.IsAny<IQueryable<EntityFixture>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAsync_Uses_Include()
    {
        var includeMock = new Mock<Func<IQueryable<EntityFixture>, IQueryable<EntityFixture>>>();
        includeMock
            .Setup(m => m(
                It.IsAny<IQueryable<EntityFixture>>()))
            .Returns((IQueryable<EntityFixture> q) =>
                q.Include(u => u.NavigationProperty));

        await ReadonlyRepo.GetAsync(
            u => u.Id == 1,
            include: includeMock.Object);

        includeMock.Verify(
            m => m(It.IsAny<IQueryable<EntityFixture>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAsync_Throws_WithNullQuery()
    {
        var ex = await Record.ExceptionAsync(() => ReadonlyRepo.GetAsync(null!));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public async Task AnyAsync_ReturnsTrue_WhenEntityExists()
    {
        var id = FillRepoAndReturnId();
        var result = await ReadonlyRepo.AnyAsync(u => u.Id == id);
        Assert.True(result);
    }

    [Fact]
    public async Task AnyAsync_Throws_WithNullQuery()
    {
        var ex = await Record.ExceptionAsync(() => ReadonlyRepo.AnyAsync(null!));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public async Task ListAsync_ReturnsAllEntities_WhenQueryIsNull()
    {
        FillRepoAndReturnId();
        var result = await ReadonlyRepo.ListAsync();
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ListAsync_ReturnsMatchingEntities_WhenQueryIsNotNull()
    {
        var id = FillRepoAndReturnId();
        var result = await ReadonlyRepo.ListAsync(u => u.Id == id);
        Assert.NotEmpty(result);
        Assert.All(result, u => Assert.Equal(id, u.Id));
    }

    [Fact]
    public async Task ListAsync_Uses_OrderBy()
    {
        var orderByMock = new Mock<Func<IQueryable<EntityFixture>, IOrderedQueryable<EntityFixture>>>();
        orderByMock
            .Setup(m => m(
                It.IsAny<IQueryable<EntityFixture>>()))
            .Returns((IQueryable<EntityFixture> q) =>
                q.OrderBy(u => u.Id));

        await ReadonlyRepo.ListAsync(orderBy: orderByMock.Object);

        orderByMock.Verify(
            m => m(It.IsAny<IQueryable<EntityFixture>>()),
            Times.Once);
    }

    [Fact]
    public async Task ListAsync_Uses_Include()
    {
        var includeMock = new Mock<Func<IQueryable<EntityFixture>, IQueryable<EntityFixture>>>();
        includeMock
            .Setup(m => m(
                It.IsAny<IQueryable<EntityFixture>>()))
            .Returns((IQueryable<EntityFixture> q) =>
                q.Include(u => u.NavigationProperty));

        await ReadonlyRepo.ListAsync(include: includeMock.Object);

        includeMock.Verify(
            m => m(It.IsAny<IQueryable<EntityFixture>>()),
            Times.Once);
    }
}

using Corely.DataAccess.Interfaces.Repos;
using System.Linq.Expressions;

namespace Corely.DataAccess.Mock.Repos;

public class MockReadonlyRepo<TEntity>
    : IReadonlyRepo<TEntity>
    where TEntity : class
{
    private readonly MockRepo<TEntity> _mockRepo;

    public MockReadonlyRepo(IRepo<TEntity> mockRepo)
    {
        // Use the same Entities list for all mocks to simulate a single data store
        _mockRepo = (MockRepo<TEntity>)mockRepo;
    }

    public virtual async Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> query,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null) => await _mockRepo.GetAsync(query, orderBy, include);

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> query) => await _mockRepo.AnyAsync(query);

    public virtual async Task<List<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? query = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null) => await _mockRepo.ListAsync(query, orderBy, include);
}

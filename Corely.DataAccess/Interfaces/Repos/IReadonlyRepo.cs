using System.Linq.Expressions;

namespace Corely.DataAccess.Interfaces.Repos;

public interface IReadonlyRepo<TEntity>
    where TEntity : class
{
    Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> query,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> query);

    Task<List<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? query = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);
}

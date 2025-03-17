namespace Corely.DataAccess.Interfaces.Repos;

public interface IRepo<TEntity>
    : IReadonlyRepo<TEntity>
    where TEntity : class
{
    Task<TEntity> CreateAsync(TEntity entity);

    Task CreateAsync(params TEntity[] entities);

    Task UpdateAsync(TEntity entity, Func<TEntity, bool> query);

    Task DeleteAsync(TEntity entity);
}

using Corely.DataAccess.Interfaces.Entities;

namespace Corely.DataAccess.Interfaces.Repos;

public interface IRepo<T>
    : IReadonlyRepo<T>
    where T : class, IHasIdPk
{
    Task<int> CreateAsync(T entity);

    Task CreateAsync(params T[] entities);

    Task UpdateAsync(T entity);

    Task DeleteAsync(T entity);

    Task DeleteAsync(int id);
}

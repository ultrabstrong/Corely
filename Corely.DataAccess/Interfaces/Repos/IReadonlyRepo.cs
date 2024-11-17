using Corely.DataAccess.Interfaces.Entities;
using System.Linq.Expressions;

namespace Corely.DataAccess.Interfaces.Repos
{
    public interface IReadonlyRepo<T>
        where T : class, IHasIdPk
    {
        Task<T?> GetAsync(int id);

        Task<T?> GetAsync(
            Expression<Func<T, bool>> query,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null);
    }
}

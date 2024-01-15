using System.Linq.Expressions;

namespace Corely.Domain.Repos
{
    public interface IRepoExtendedGet<T> : IRepo<T>
    {
        Task<T?> GetAsync(Expression<Func<T, bool>> query,
            Expression<Func<T, object>>? include = null);
    }
}

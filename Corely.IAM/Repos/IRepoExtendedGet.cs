using System.Linq.Expressions;

namespace Corely.IAM.Repos
{
    public interface IRepoExtendedGet<T> : IRepo<T>
    {
        Task<T?> GetAsync(
            Expression<Func<T, bool>> query,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null);
    }
}

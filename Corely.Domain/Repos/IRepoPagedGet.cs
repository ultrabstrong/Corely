using Corely.Common.Models;

namespace Corely.Domain.Repos
{
    public interface IRepoPagedGet<T> : IRepo<T>
    {
        Task<PagedResult<T>> GetPaged(PagedResult<T> curPage);
    }
}

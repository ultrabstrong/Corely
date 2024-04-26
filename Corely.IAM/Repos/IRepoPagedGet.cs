using Corely.Common.Models;

namespace Corely.IAM.Repos
{
    public interface IRepoPagedGet<T> : IRepo<T>
    {
        Task<PagedResult<T>> GetPaged(PagedResult<T> curPage);
    }
}

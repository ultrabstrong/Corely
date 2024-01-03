using Corely.Common.Models.Results;

namespace Corely.Domain.Repos
{
    public interface IRepoExtendedGet<T> : IRepo<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<PagedResult<T>> GetPaged(PagedResult<T> curPage);
    }
}

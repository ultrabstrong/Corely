using Corely.Common.Models.Results;

namespace Corely.Domain.Repos
{
    public interface IRepoExtendedGet<T> : IRepo<T>
    {
        IEnumerable<T> GetAll();
        PagedResult<T> GetPaged(PagedResult<T> curPage);
    }
}

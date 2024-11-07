using Corely.DataAccess.Interfaces.Entities;
using Corely.DataAccess.Interfaces.Repos;
using System.Linq.Expressions;

namespace Corely.DataAccess.Mock.Repos
{
    public class MockRepoExtendedGet<T>
        : IRepoExtendedGet<T>
        where T : IHasIdPk
    {
        private readonly MockRepo<T> _mockRepo;

        public MockRepoExtendedGet(IRepo<T> mockRepo)
        {
            _mockRepo = (MockRepo<T>)mockRepo;
        }

        public async Task<int> CreateAsync(T entity) => await _mockRepo.CreateAsync(entity);

        public async Task<T?> GetAsync(int id) => await _mockRepo.GetAsync(id);

        public async Task UpdateAsync(T entity) => await _mockRepo.UpdateAsync(entity);

        public virtual async Task<T?> GetAsync(
            Expression<Func<T, bool>> query,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            ArgumentNullException.ThrowIfNull(query);
            var predicate = query.Compile();
            var queryable = _mockRepo.Entities.AsQueryable();

            if (include != null)
            {
                queryable = include(queryable);
            }

            if (orderBy != null)
            {
                queryable = orderBy(queryable);
            }

            return await Task.FromResult(queryable.FirstOrDefault(predicate));
        }

        public async Task DeleteAsync(T entity) => await _mockRepo.DeleteAsync(entity);
    }
}

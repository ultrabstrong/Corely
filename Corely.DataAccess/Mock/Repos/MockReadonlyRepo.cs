using Corely.DataAccess.Interfaces.Entities;
using Corely.DataAccess.Interfaces.Repos;
using System.Linq.Expressions;

namespace Corely.DataAccess.Mock.Repos
{
    public class MockReadonlyRepo<T>
        : IReadonlyRepo<T>
        where T : class, IHasIdPk
    {
        private readonly MockRepo<T> _mockRepo;

        public MockReadonlyRepo(IRepo<T> mockRepo)
        {
            // Use the same Entities list for all mocks to simulate a single data store
            _mockRepo = (MockRepo<T>)mockRepo;
        }

        public virtual async Task<T?> GetAsync(int id) => await _mockRepo.GetAsync(id);

        public virtual async Task<T?> GetAsync(
            Expression<Func<T, bool>> query,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null) => await _mockRepo.GetAsync(query, orderBy, include);

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> query) => await _mockRepo.AnyAsync(query);
    }
}

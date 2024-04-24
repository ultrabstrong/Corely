using Corely.Domain.Entities;
using Corely.Domain.Repos;

namespace Corely.DataAccess.Repos
{
    internal class MockReadonlyRepo<T>
        : IReadonlyRepo<T>
        where T : class, IHasIdPk
    {
        private readonly IRepo<T> _mockRepo;

        public MockReadonlyRepo(IRepo<T> mockRepo)
        {
            _mockRepo = mockRepo;
        }

        public virtual async Task<T?> GetAsync(int id) => await _mockRepo.GetAsync(id);
    }
}

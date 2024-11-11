using Corely.DataAccess.Interfaces.Entities;
using Corely.DataAccess.Interfaces.Repos;

namespace Corely.DataAccess.Mock.Repos
{
    public class MockReadonlyRepo<T>
        : IReadonlyRepo<T>
        where T : class, IHasIdPk
    {
        private readonly IRepo<T> _mockRepo;

        public MockReadonlyRepo(IRepo<T> mockRepo)
        {
            // Use the same Entities list for all mocks to simulate a single data store
            _mockRepo = mockRepo;
        }

        public virtual async Task<T?> GetAsync(int id) => await _mockRepo.GetAsync(id);
    }
}

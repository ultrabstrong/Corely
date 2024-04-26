using Corely.IAM.Entities;
using Corely.IAM.Repos;

namespace Corely.DataAccess.Mock.Repos
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

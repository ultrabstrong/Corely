using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;

namespace Corely.DataAccess.Repos.Auth
{
    internal class MockBasicAuthRepo : IAuthRepo<BasicAuthEntity>
    {
        private readonly List<BasicAuthEntity> _auths = [];

        public Task Create(BasicAuthEntity entity)
        {
            _auths.Add(entity);
            return Task.CompletedTask;
        }

        public Task<BasicAuthEntity?> Get(int id)
        {
            return Task.FromResult(_auths.FirstOrDefault(a => a.Id == id));
        }

        public Task<BasicAuthEntity?> GetByUserId(int userId)
        {
            return Task.FromResult(_auths.FirstOrDefault(a => a.UserId == userId));
        }

        public Task<BasicAuthEntity?> GetByUserName(string userName)
        {
            return Task.FromResult(_auths.FirstOrDefault(a => a.Username == userName));
        }

        public Task Update(BasicAuthEntity entity)
        {
            var index = _auths.FindIndex(a => a.Id == entity.Id);
            if (index > -1) { _auths[index] = entity; }
            return Task.CompletedTask;
        }

        public Task Delete(BasicAuthEntity entity)
        {
            _auths.Remove(entity);
            return Task.CompletedTask;
        }
    }
}

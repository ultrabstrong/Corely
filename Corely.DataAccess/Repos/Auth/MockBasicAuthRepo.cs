using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;

namespace Corely.DataAccess.Repos.Auth
{
    internal class MockBasicAuthRepo : IAuthRepo<BasicAuthEntity>
    {
        private readonly List<BasicAuthEntity> _auths = [];

        public void Create(BasicAuthEntity entity)
        {
            _auths.Add(entity);
        }

        public BasicAuthEntity? Get(int id)
        {
            return _auths.FirstOrDefault(a => a.Id == id);
        }

        public BasicAuthEntity? GetByUserId(int userId)
        {
            return _auths.FirstOrDefault(a => a.UserId == userId);
        }

        public BasicAuthEntity? GetByUserName(string userName)
        {
            return _auths.FirstOrDefault(a => a.Username == userName);
        }

        public void Update(BasicAuthEntity entity)
        {
            var existing = _auths.FirstOrDefault(a => a.Id == entity.Id);
            if (existing != null)
            {
                _auths.Remove(existing);
                _auths.Add(entity);
            }
        }
        public void Delete(BasicAuthEntity entity)
        {
            _auths.Remove(entity);
        }
    }
}

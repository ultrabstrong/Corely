using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;

namespace Corely.DataAccess.Repos.User
{
    internal class MockUserRepo : IUserRepo
    {
        private readonly List<UserEntity> _users = new();

        public void Create(UserEntity entity)
        {
            _users.Add(entity);
        }

        public UserEntity? Get(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public UserEntity? GetByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email);
        }

        public UserEntity? GetByUserName(string userName)
        {
            return _users.FirstOrDefault(u => u.Username == userName);
        }

        public UserEntity? GetWithDetailsById(int userId)
        {
            return _users.FirstOrDefault(u => u.Id == userId);
        }

        public UserEntity? GetWithDetailsByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email);
        }

        public UserEntity? GetWithDetailsByUserName(string userName)
        {
            return _users.FirstOrDefault(u => u.Username == userName);
        }

        public void Update(UserEntity entity)
        {
            var existing = _users.FirstOrDefault(u => u.Id == entity.Id);
            if (existing != null)
            {
                _users.Remove(existing);
                _users.Add(entity);
            }
        }

        public void Delete(UserEntity entity)
        {
            _users.Remove(entity);
        }

        public bool DoesUserExist(string userName, string email)
        {
            return _users.Any(u => u.Username == userName || u.Email == email);
        }
    }
}

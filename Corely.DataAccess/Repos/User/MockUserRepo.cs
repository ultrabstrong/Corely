using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;

namespace Corely.DataAccess.Repos.User
{
    internal class MockUserRepo : IUserRepo
    {
        private readonly List<UserEntity> _users = [];

        public Task Create(UserEntity entity)
        {
            _users.Add(entity);
            return Task.CompletedTask;
        }

        public Task<UserEntity?> Get(int id)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
        }

        public Task<UserEntity?> GetByUserName(string userName)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Username == userName));
        }

        public Task<UserEntity?> GetByEmail(string email)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Email == email));
        }

        public Task<UserEntity?> GetByUserNameOrEmail(string userName, string email)
        {
            return Task.FromResult(_users
                .FirstOrDefault(u => u.Username == userName || u.Email == email));
        }

        public Task<UserEntity?> GetWithDetailsById(int userId)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Id == userId));
        }

        public Task<UserEntity?> GetWithDetailsByEmail(string email)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Email == email));
        }

        public Task<UserEntity?> GetWithDetailsByUserName(string userName)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Username == userName));
        }

        public Task Update(UserEntity entity)
        {
            var index = _users.FindIndex(u => u.Id == entity.Id);
            if (index > -1) { _users[index] = entity; }
            return Task.CompletedTask;
        }

        public Task Delete(UserEntity entity)
        {
            _users.Remove(entity);
            return Task.CompletedTask;
        }
    }
}

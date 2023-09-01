using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;

namespace Corely.DataAccess.Repos.User
{
    internal class EFUserRepo : IUserRepo
    {
        public void Create(UserEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(UserEntity entity)
        {
            throw new NotImplementedException();
        }

        public UserEntity Get(int id)
        {
            throw new NotImplementedException();
        }

        public UserEntity GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public UserEntity GetByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public UserEntity GetWithDetailsByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public UserEntity GetWithDetailsById(int userId)
        {
            throw new NotImplementedException();
        }

        public UserEntity GetWithDetailsByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public void Update(UserEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}

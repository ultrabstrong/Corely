using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;

namespace Corely.DataAccess.Repos.Auth
{
    internal class EFBasicAuthRepo : IAuthRepo<BasicAuthEntity>
    {
        private readonly string _connectionString;

        public EFBasicAuthRepo(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Create(BasicAuthEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(BasicAuthEntity entity)
        {
            throw new NotImplementedException();
        }

        public BasicAuthEntity Get(int id)
        {
            throw new NotImplementedException();
        }

        public BasicAuthEntity GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public BasicAuthEntity GetByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public BasicAuthEntity GetByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public void Update(BasicAuthEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}

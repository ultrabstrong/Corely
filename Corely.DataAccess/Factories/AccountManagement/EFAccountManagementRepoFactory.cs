using Corely.DataAccess.Repos.Auth;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;

namespace Corely.DataAccess.Factories.AccountManagement
{
    internal class EFAccountManagementRepoFactory : IAccountManagementRepoFactory
    {
        private readonly string _connection;

        public EFAccountManagementRepoFactory(string connection)
        {
            _connection = connection;
        }

        public IAuthRepo<BasicAuthEntity> GetBasicAuthRepo()
        {
            return new EFBasicAuthRepo(_connection);
        }

        public IUserRepo GetUserRepo()
        {
            throw new NotImplementedException();
        }
    }
}

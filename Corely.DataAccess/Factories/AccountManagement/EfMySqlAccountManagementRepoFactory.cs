using Corely.DataAccess.DataAccess.EntityFramework;
using Corely.DataAccess.Repos.Auth;
using Corely.DataAccess.Repos.User;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;
using Microsoft.EntityFrameworkCore;

namespace Corely.DataAccess.Factories.AccountManagement
{
    internal class EfMySqlAccountManagementRepoFactory : IAccountManagementRepoFactory
    {
        private readonly string _connection;

        public EfMySqlAccountManagementRepoFactory(string connection)
        {
            _connection = connection;
        }

        private AccountManagementDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<AccountManagementDbContext> optionsBuilder = new();
            optionsBuilder.UseMySql(_connection, ServerVersion.AutoDetect(_connection));
            return new(optionsBuilder.Options);
        }

        public IAuthRepo<BasicAuthEntity> GetBasicAuthRepo()
        {
            return new EFBasicAuthRepo(CreateDbContext());
        }

        public IUserRepo GetUserRepo()
        {
            return new EFUserRepo(CreateDbContext());
        }
    }
}

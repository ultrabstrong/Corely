using Corely.DataAccess.EntityFramework.AccountManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Corely.DataAccessMigrations
{
    public class AccountManagementDesignTimeDbContextFactory : IDesignTimeDbContextFactory<AccountManagementDbContext>
    {

        public AccountManagementDbContext CreateDbContext(string[] args)
        {
            var configuration = new EFMySqlConfiguration(ConfigurationProvider.GetConnectionString());
            var optionsBuilder = new DbContextOptionsBuilder<AccountManagementDbContext>();
            configuration.Configure(optionsBuilder);
            return new AccountManagementDbContext(configuration);
        }
    }
}

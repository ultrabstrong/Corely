using Corely.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Corely.DataAccessMigrations
{
    public class AccountManagementDbContextFactory : IDesignTimeDbContextFactory<AccountManagementDbContext>
    {

        public AccountManagementDbContext CreateDbContext(string[] args)
        {
            var configuration = new EFConfiguration(ConfigurationProvider.GetConnectionString());
            var optionsBuilder = new DbContextOptionsBuilder<AccountManagementDbContext>();
            configuration.Configure(optionsBuilder);
            return new AccountManagementDbContext(configuration);
        }
    }
}

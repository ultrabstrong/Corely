using Corely.Domain.Entities.Auth;
using Corely.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Corely.DataAccess.DataAccess.EntityFramework
{
    internal class AccountManagementDbContext : DbContext
    {
        public AccountManagementDbContext(
            DbContextOptions<AccountManagementDbContext> options)
            : base(options)
        {

        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<BasicAuthEntity> BasicAuths { get; set; }
    }
}

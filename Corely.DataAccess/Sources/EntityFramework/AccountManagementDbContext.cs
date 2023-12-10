using Corely.DataAccess.Sources.EntityFramework.EntityConfigurations.Users;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Corely.DataAccess.Sources.EntityFramework
{
    internal class AccountManagementDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<BasicAuthEntity> BasicAuths { get; set; }

        public AccountManagementDbContext()
            : base() { }

        public AccountManagementDbContext(
            DbContextOptions<AccountManagementDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserDetailsEntityConfiguration());
        }
    }
}

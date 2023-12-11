using Corely.DataAccess.DataSources.EntityFramework.Configurations.Auth;
using Corely.DataAccess.DataSources.EntityFramework.Configurations.Users;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Corely.DataAccess.DataSources.EntityFramework
{
    internal class AccountManagementDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserDetailsEntity> UserDetails { get; set; }
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

            modelBuilder.ApplyConfiguration(new BasicAuthEntityConfiguration());
        }
    }
}

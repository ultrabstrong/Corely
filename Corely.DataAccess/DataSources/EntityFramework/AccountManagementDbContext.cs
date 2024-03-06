using Corely.DataAccess.Connections;
using Corely.DataAccess.DataSources.EntityFramework.Configurations.Auth;
using Corely.DataAccess.DataSources.EntityFramework.Configurations.Users;
using Corely.Domain.Entities.Accounts;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Corely.DataAccess.DataSources.EntityFramework
{
    internal class AccountManagementDbContext : DbContext
    {
        private readonly IEFConfiguration _configuration;

        public AccountManagementDbContext(IEFConfiguration configuration)
            : base()
        {
            _configuration = configuration;
        }

        public DbSet<AccountEntity> Accounts { get; set; } = null!;
        public DbSet<UserEntity> Users { get; set; } = null!;
        public DbSet<UserDetailsEntity> UserDetails { get; set; } = null!;
        public DbSet<BasicAuthEntity> BasicAuths { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _configuration.Configure(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserDetailsEntityConfiguration());

            modelBuilder.ApplyConfiguration(new BasicAuthEntityConfiguration());
        }
    }
}

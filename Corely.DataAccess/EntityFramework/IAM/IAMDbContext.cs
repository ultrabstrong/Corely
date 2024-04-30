using Corely.DataAccess.EntityFramework.IAM.EntityConfigurations.Accounts;
using Corely.DataAccess.EntityFramework.IAM.EntityConfigurations.Auth;
using Corely.DataAccess.EntityFramework.IAM.EntityConfigurations.Users;
using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Auth.Entities;
using Corely.IAM.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace Corely.DataAccess.EntityFramework.IAM
{
    public class IAMDbContext : DbContext
    {
        private readonly IEFConfiguration _configuration;

        public IAMDbContext(IEFConfiguration configuration)
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
            modelBuilder.ApplyConfiguration(new AccountEntityConfiguration(_configuration.GetDbTypes()));

            modelBuilder.ApplyConfiguration(new UserEntityConfiguration(_configuration.GetDbTypes()));
            modelBuilder.ApplyConfiguration(new UserDetailsEntityConfiguration(_configuration.GetDbTypes()));

            modelBuilder.ApplyConfiguration(new BasicAuthEntityConfiguration(_configuration.GetDbTypes()));
        }
    }
}

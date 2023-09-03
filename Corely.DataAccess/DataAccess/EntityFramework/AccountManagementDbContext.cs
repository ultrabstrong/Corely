using Corely.Domain.Entities.Auth;
using Corely.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Corely.DataAccess.DataAccess.EntityFramework
{
    internal class AccountManagementDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<BasicAuthEntity> BasicAuths { get; set; }

        public AccountManagementDbContext(
            DbContextOptions<AccountManagementDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(e => e.Id).HasColumnName("UserID");
                entity.Property(e => e.Username).HasMaxLength(100);
            });

            modelBuilder.Entity<UserDetailsEntity>(entity =>
            {
                entity.ToTable("UserDetails");
            });
        }
    }
}

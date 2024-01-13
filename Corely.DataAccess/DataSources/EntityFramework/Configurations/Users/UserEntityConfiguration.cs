using Corely.Domain.Constants.Users;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.DataSources.EntityFramework.Configurations.Users
{
    internal class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            GenericEntityTypeConfiguration.Configure(builder);

            builder.Property(e => e.Enabled)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(e => e.Username)
                .HasMaxLength(UserConstants.USERNAME_MAX_LENGTH)
                .IsRequired();

            builder.HasIndex(e => e.Username)
                .IsUnique();

            builder.Property(e => e.Email)
                .HasMaxLength(UserConstants.EMAIL_MAX_LENGTH)
                .IsRequired();

            builder.HasIndex(e => e.Email)
                .IsUnique();

            builder.HasOne(p => p.Details)
                .WithOne(d => d.User)
                .HasForeignKey<UserDetailsEntity>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.BasicAuth)
                .WithOne(d => d.User)
                .HasForeignKey<BasicAuthEntity>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

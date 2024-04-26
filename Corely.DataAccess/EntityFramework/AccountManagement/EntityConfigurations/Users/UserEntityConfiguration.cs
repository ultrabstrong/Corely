using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Constants.Users;
using Corely.IAM.Entities.Auth;
using Corely.IAM.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.EntityFramework.AccountManagement.EntityConfigurations.Users
{
    internal sealed class UserEntityConfiguration : EntityConfigurationBase<UserEntity>
    {
        public UserEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
        {
        }

        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            ConfigureGenericTypes(builder);

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

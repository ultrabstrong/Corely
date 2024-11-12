using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Auth.Entities;
using Corely.IAM.Users.Constants;
using Corely.IAM.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.IAM.DataAccess.EntityFramework.EntityConfigurations.Users
{
    internal sealed class UserEntityConfiguration : EntityConfigurationBase<UserEntity>
    {
        public UserEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
        {
        }

        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            ConfigureGenericTypes(builder);

            builder.Property(e => e.Disabled)
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

            builder.HasMany(e => e.SymmetricKeys)
                .WithOne()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.AsymmetricKeys)
                .WithOne()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.BasicAuth)
                .WithOne(d => d.User)
                .HasForeignKey<BasicAuthEntity>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

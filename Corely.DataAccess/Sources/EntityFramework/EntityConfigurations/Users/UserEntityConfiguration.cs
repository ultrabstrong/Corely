using Corely.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.Sources.EntityFramework.EntityConfigurations.Users
{
    internal class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.CreatedUtc)
                .HasDefaultValueSql(SqlConstants.GETUTCDATE)
                .IsRequired();

            builder.Property(e => e.Enabled)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(e => e.Username)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(e => e.Email)
                .HasMaxLength(254) // RFC 5321 standard
                .IsRequired();

            builder.HasOne(p => p.Details)
                .WithOne(d => d.User)
                .HasForeignKey<UserDetailsEntity>(p => p.UserId);

        }
    }
}

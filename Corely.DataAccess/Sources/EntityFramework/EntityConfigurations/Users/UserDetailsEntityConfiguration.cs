using Corely.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.Sources.EntityFramework.EntityConfigurations.Users
{
    internal class UserDetailsEntityConfiguration : IEntityTypeConfiguration<UserDetailsEntity>
    {
        public void Configure(EntityTypeBuilder<UserDetailsEntity> builder)
        {
            builder.ToTable("UserDetails");

            builder.HasKey(e => e.UserId);

            builder.Property(e => e.Name)
                .HasMaxLength(4000); // https://www.kalzumeus.com/2010/06/17/falsehoods-programmers-believe-about-names/
        }
    }
}

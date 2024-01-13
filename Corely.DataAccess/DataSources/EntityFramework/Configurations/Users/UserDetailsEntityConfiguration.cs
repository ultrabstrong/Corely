using Corely.Domain.Constants.Users;
using Corely.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.DataSources.EntityFramework.Configurations.Users
{
    internal class UserDetailsEntityConfiguration : IEntityTypeConfiguration<UserDetailsEntity>
    {
        public void Configure(EntityTypeBuilder<UserDetailsEntity> builder)
        {
            GenericEntityTypeConfiguration.Configure(builder);

            builder.HasKey(e => e.UserId);
            builder.Property(e => e.UserId)
                .ValueGeneratedNever();

            builder.Property(e => e.Name)
                .HasMaxLength(UserDetailsConstants.NAME_MAX_LENGTH);
        }
    }
}

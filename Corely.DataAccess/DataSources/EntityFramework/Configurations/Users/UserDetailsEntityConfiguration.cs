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
            BaseEntityTypeConfiguration.Configure(builder);

            builder.Property(e => e.Name)
                .HasMaxLength(UserDetailsConstants.NAME_MAX_LENGTH);
        }
    }
}

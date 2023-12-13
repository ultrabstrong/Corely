using Corely.Domain.Constants.Auth;
using Corely.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.DataSources.EntityFramework.Configurations.Auth
{
    internal class BasicAuthEntityConfiguration : IEntityTypeConfiguration<BasicAuthEntity>
    {
        public void Configure(EntityTypeBuilder<BasicAuthEntity> builder)
        {
            BaseEntityTypeConfiguration.Configure(builder);

            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(BasicAuthConstants.USERNAME_MAX_LENGTH);

            builder.HasIndex(x => x.Username)
                .IsUnique();

            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(BasicAuthConstants.PASSWORD_MAX_LENGTH);
        }
    }
}

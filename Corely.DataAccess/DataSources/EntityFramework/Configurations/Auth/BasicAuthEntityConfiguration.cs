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
                .HasMaxLength(30);

            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(250); // Hashed password with encoded salt / other info
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.Sources.EntityFramework.EntityConfigurations.Auth
{
    internal class BasicAuthEntityConfiguration : IEntityTypeConfiguration<BasicAuthEntityConfiguration>
    {
        public void Configure(EntityTypeBuilder<BasicAuthEntityConfiguration> builder)
        {
            builder.ToTable("BasicAuth");
        }
    }
}

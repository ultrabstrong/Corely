using Corely.DataAccess.Connections;
using Corely.DataAccess.EntityFramework.Configurations;
using Corely.Domain.Constants.Auth;
using Corely.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.EntityFramework.Configurations.Auth
{
    internal class BasicAuthEntityConfiguration : EntityConfigurationBase<BasicAuthEntity>
    {
        public BasicAuthEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
        {
        }

        public override void Configure(EntityTypeBuilder<BasicAuthEntity> builder)
        {
            ConfigureGenericTypes(builder);

            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(BasicAuthConstants.PASSWORD_MAX_LENGTH);
        }
    }
}

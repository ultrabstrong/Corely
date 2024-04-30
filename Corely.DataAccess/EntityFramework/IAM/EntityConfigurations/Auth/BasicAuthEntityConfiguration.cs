using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Auth.Constants;
using Corely.IAM.Auth.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.EntityFramework.IAM.EntityConfigurations.Auth
{
    internal sealed class BasicAuthEntityConfiguration : EntityConfigurationBase<BasicAuthEntity>
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

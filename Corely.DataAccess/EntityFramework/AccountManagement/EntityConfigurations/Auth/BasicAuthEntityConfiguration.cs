using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Constants.Auth;
using Corely.IAM.Entities.Auth;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.EntityFramework.AccountManagement.EntityConfigurations.Auth
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

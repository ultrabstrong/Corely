using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.BasicAuths.Constants;
using Corely.IAM.BasicAuths.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.IAM.DataAccess.EntityFramework.EntityConfigurations.Auth
{
    internal sealed class BasicAuthEntityConfiguration : EntityConfigurationBase<BasicAuthEntity>
    {
        public BasicAuthEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
        {
        }

        protected override void ConfigureInternal(EntityTypeBuilder<BasicAuthEntity> builder)
        {
            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(BasicAuthConstants.PASSWORD_MAX_LENGTH);
        }
    }
}

using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Security;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.EntityFramework.AccountManagement.EntityConfigurations.Security
{
    internal sealed class SymmetricKeyConfiguration : EntityConfigurationBase<SymmetricKeyEntity>
    {
        public SymmetricKeyConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
        {
        }

        public override void Configure(EntityTypeBuilder<SymmetricKeyEntity> builder)
        {
            ConfigureGenericTypes(builder);

            builder.Property(m => m.Key)
                .IsRequired()
                .HasMaxLength(SymmetricKeyConstants.KEY_MAX_LENGTH);
        }
    }
}

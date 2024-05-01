using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Security.Constants;
using Corely.IAM.Security.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.EntityFramework.IAM.EntityConfigurations.Security
{
    internal sealed class SymmetricKeyEntityConfiguration : EntityConfigurationBase<SymmetricKeyEntity>
    {
        public SymmetricKeyEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
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

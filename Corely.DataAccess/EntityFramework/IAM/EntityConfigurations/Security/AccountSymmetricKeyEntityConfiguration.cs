using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Security.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.EntityFramework.IAM.EntityConfigurations.Security
{
    internal class AccountSymmetricKeyEntityConfiguration : EntityConfigurationBase<AccountSymmetricKeyEntity>
    {
        public AccountSymmetricKeyEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
        {
        }

        public override void Configure(EntityTypeBuilder<AccountSymmetricKeyEntity> builder)
        {
            ConfigureGenericTypes(builder);

            builder.HasKey(e => e.AccountId);
            builder.Property(e => e.AccountId)
                .ValueGeneratedNever();

            builder.Property(m => m.Key)
                .IsRequired()
                .HasMaxLength(SymmetricKeyConstants.KEY_MAX_LENGTH);
        }
    }
}

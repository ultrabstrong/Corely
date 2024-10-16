using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Security.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.EntityFramework.IAM.EntityConfigurations.Accounts
{
    internal class AccountSymmetricKeyEntityConfiguration : EntityConfigurationBase<AccountSymmetricKeyEntity>
    {
        public AccountSymmetricKeyEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
        {
        }

        public override void Configure(EntityTypeBuilder<AccountSymmetricKeyEntity> builder)
        {
            ConfigureGenericTypes(builder);

            builder.HasIndex(e => new { e.AccountId, e.KeyUsedFor })
              .IsUnique();

            builder.Property(m => m.Key)
                .IsRequired()
                .HasMaxLength(SymmetricKeyConstants.KEY_MAX_LENGTH);


            builder.Property(m => m.Version)
                .IsRequired();

            builder.Property(m => m.ProviderTypeCode)
                .IsRequired();
        }
    }
}

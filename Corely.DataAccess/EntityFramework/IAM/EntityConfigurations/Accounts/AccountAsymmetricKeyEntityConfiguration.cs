using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Accounts.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.EntityFramework.IAM.EntityConfigurations.Accounts
{
    internal class AccountAsymmetricKeyEntityConfiguration : EntityConfigurationBase<AccountAsymmetricKeyEntity>
    {
        public AccountAsymmetricKeyEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
        {
        }

        public override void Configure(EntityTypeBuilder<AccountAsymmetricKeyEntity> builder)
        {
            ConfigureGenericTypes(builder);

            builder.HasIndex(e => new { e.AccountId, e.KeyUsedFor })
                .IsUnique();

            builder.Property(m => m.KeyUsedFor)
                .HasConversion<string>();

            builder.Property(m => m.ProviderTypeCode)
                .IsRequired();

            builder.Property(m => m.Version)
                .IsRequired();

            builder.Property(m => m.PublicKey)
                .IsRequired();

            builder.Property(m => m.PrivateKey)
                .IsRequired();
        }
    }
}

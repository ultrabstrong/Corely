using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Security.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.EntityFramework.IAM.EntityConfigurations.Security
{
    internal class AccountAsymmetricKeyEntityConfiguration : EntityConfigurationBase<AccountAsymmetricKeyEntity>
    {
        public AccountAsymmetricKeyEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
        {
        }

        public override void Configure(EntityTypeBuilder<AccountAsymmetricKeyEntity> builder)
        {
            ConfigureGenericTypes(builder);

            builder.HasKey(e => e.AccountId);
            builder.Property(e => e.AccountId)
                .ValueGeneratedNever();

            builder.Property(m => m.PublicKey)
                .IsRequired();

            builder.Property(m => m.PrivateKey)
                .IsRequired();
        }
    }
}

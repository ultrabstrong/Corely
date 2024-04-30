using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Accounts.Constants;
using Corely.IAM.Accounts.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.EntityFramework.IAM.EntityConfigurations.Accounts
{
    internal sealed class AccountEntityConfiguration : EntityConfigurationBase<AccountEntity>
    {
        public AccountEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
        {
        }

        public override void Configure(EntityTypeBuilder<AccountEntity> builder)
        {
            ConfigureGenericTypes(builder);

            builder.Property(e => e.AccountName)
                .IsRequired()
                .HasMaxLength(AccountConstants.ACCOUNT_NAME_MAX_LENGTH);

            builder.HasIndex(e => e.AccountName)
                .IsUnique();

            builder.HasMany(e => e.Users)
                .WithMany(e => e.Accounts);

            builder.HasOne(e => e.SymmetricKey);
        }
    }
}

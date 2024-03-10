using Corely.DataAccess.Connections;
using Corely.Domain.Constants.Accounts;
using Corely.Domain.Entities.Accounts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.DataSources.EntityFramework.Configurations.Accounts
{
    internal class AccountEntityConfiguration : EntityConfigurationBase<AccountEntity>
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
        }
    }
}

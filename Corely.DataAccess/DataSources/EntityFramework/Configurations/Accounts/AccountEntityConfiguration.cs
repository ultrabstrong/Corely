using Corely.Domain.Constants.Accounts;
using Corely.Domain.Entities.Accounts;
using Microsoft.EntityFrameworkCore;

namespace Corely.DataAccess.DataSources.EntityFramework.Configurations.Accounts
{
    internal class AccountEntityConfiguration : IEntityTypeConfiguration<AccountEntity>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<AccountEntity> builder)
        {
            GenericEntityTypeConfiguration.Configure(builder);

            builder.Property(e => e.AccountName)
                .IsRequired()
                .HasMaxLength(AccountConstants.ACCOUNT_NAME_MAX_LENGTH);

            builder.HasIndex(e => e.AccountName)
                .IsUnique();
        }
    }
}

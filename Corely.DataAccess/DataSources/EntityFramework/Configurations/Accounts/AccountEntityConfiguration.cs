using Corely.Domain.Constants.Accounts;
using Corely.Domain.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.DataSources.EntityFramework.Configurations.Accounts
{
    internal class AccountEntityConfiguration : IEntityTypeConfiguration<AccountEntity>
    {
        public void Configure(EntityTypeBuilder<AccountEntity> builder)
        {
            GenericEntityTypeConfiguration.Configure(builder);

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

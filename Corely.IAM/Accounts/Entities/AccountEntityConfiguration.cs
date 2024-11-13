using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Accounts.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.IAM.Accounts.Entities
{
    internal sealed class AccountEntityConfiguration : EntityConfigurationBase<AccountEntity>
    {
        public AccountEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
        {
        }

        protected override void ConfigureInternal(EntityTypeBuilder<AccountEntity> builder)
        {
            builder.Property(e => e.AccountName)
                .IsRequired()
                .HasMaxLength(AccountConstants.ACCOUNT_NAME_MAX_LENGTH);

            builder.HasIndex(e => e.AccountName)
                .IsUnique();

            builder.HasMany(e => e.Users)
                .WithMany(e => e.Accounts);

            builder.HasMany(e => e.SymmetricKeys)
                .WithOne()
                .HasForeignKey(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.AsymmetricKeys)
                .WithOne()
                .HasForeignKey(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

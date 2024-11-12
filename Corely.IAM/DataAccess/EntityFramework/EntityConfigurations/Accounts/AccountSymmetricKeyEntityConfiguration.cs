﻿using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Security.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.IAM.DataAccess.EntityFramework.EntityConfigurations.Accounts
{
    internal class AccountSymmetricKeyEntityConfiguration : EntityConfigurationBase<AccountSymmetricKeyEntity>
    {
        public AccountSymmetricKeyEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
        {
        }

        protected override void ConfigureInternal(EntityTypeBuilder<AccountSymmetricKeyEntity> builder)
        {
            builder.HasIndex(e => new { e.AccountId, e.KeyUsedFor })
              .IsUnique();

            builder.Property(m => m.KeyUsedFor)
                .HasConversion<string>();

            builder.Property(m => m.ProviderTypeCode)
                .IsRequired();

            builder.Property(m => m.Version)
                .IsRequired();

            builder.Property(m => m.EncryptedKey)
                .IsRequired()
                .HasMaxLength(SymmetricKeyConstants.KEY_MAX_LENGTH);
        }
    }
}

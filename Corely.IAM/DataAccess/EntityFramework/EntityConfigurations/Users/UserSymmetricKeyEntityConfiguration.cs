using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Security.Constants;
using Corely.IAM.Users.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.IAM.DataAccess.EntityFramework.EntityConfigurations.Users
{
    internal class UserSymmetricKeyEntityConfiguration : EntityConfigurationBase<UserSymmetricKeyEntity>
    {
        public UserSymmetricKeyEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
        {
        }

        public override void Configure(EntityTypeBuilder<UserSymmetricKeyEntity> builder)
        {
            ConfigureGenericTypes(builder);

            builder.HasIndex(e => new { e.UserId, e.KeyUsedFor })
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

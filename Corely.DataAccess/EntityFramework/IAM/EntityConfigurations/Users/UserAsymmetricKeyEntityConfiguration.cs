using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Users.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.EntityFramework.IAM.EntityConfigurations.Users
{
    internal class UserAsymmetricKeyEntityConfiguration : EntityConfigurationBase<UserAsymmetricKeyEntity>
    {
        public UserAsymmetricKeyEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
        {
        }

        public override void Configure(EntityTypeBuilder<UserAsymmetricKeyEntity> builder)
        {
            ConfigureGenericTypes(builder);

            builder.HasIndex(e => new { e.UserId, e.KeyUsedFor })
                .IsUnique();

            builder.Property(m => m.PublicKey)
                .IsRequired();

            builder.Property(m => m.PrivateKey)
                .IsRequired();
        }
    }
}

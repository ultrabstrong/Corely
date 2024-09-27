using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Security.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.EntityFramework.IAM.EntityConfigurations.Security
{
    internal class UserAsymmetricKeyEntityConfiguration : EntityConfigurationBase<UserAsymmetricKeyEntity>
    {
        public UserAsymmetricKeyEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
        {
        }

        public override void Configure(EntityTypeBuilder<UserAsymmetricKeyEntity> builder)
        {
            ConfigureGenericTypes(builder);

            builder.HasKey(e => e.UserId);
            builder.Property(e => e.UserId)
                .ValueGeneratedNever();

            builder.Property(m => m.PublicKey)
                .IsRequired();

            builder.Property(m => m.PrivateKey)
                .IsRequired();
        }
    }
}

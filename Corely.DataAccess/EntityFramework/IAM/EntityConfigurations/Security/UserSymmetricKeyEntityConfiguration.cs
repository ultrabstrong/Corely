using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Security.Constants;
using Corely.IAM.Users.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.EntityFramework.IAM.EntityConfigurations.Security
{
    internal class UserSymmetricKeyEntityConfiguration : EntityConfigurationBase<UserSymmetricKeyEntity>
    {
        public UserSymmetricKeyEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
        {
        }

        public override void Configure(EntityTypeBuilder<UserSymmetricKeyEntity> builder)
        {
            ConfigureGenericTypes(builder);

            builder.HasKey(e => e.UserId);
            builder.Property(e => e.UserId)
                .ValueGeneratedNever();

            builder.Property(m => m.Key)
                .IsRequired()
                .HasMaxLength(SymmetricKeyConstants.KEY_MAX_LENGTH);
        }
    }
}

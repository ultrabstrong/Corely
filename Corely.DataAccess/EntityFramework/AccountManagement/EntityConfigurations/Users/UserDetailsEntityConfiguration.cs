using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Users.Constants;
using Corely.IAM.Users.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.EntityFramework.AccountManagement.EntityConfigurations.Users
{
    internal sealed class UserDetailsEntityConfiguration : EntityConfigurationBase<UserDetailsEntity>
    {
        public UserDetailsEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
        {
        }

        public override void Configure(EntityTypeBuilder<UserDetailsEntity> builder)
        {
            ConfigureGenericTypes(builder);

            builder.HasKey(e => e.UserId);
            builder.Property(e => e.UserId)
                .ValueGeneratedNever();

            builder.Property(e => e.Name)
                .HasMaxLength(UserDetailsConstants.NAME_MAX_LENGTH);
        }
    }
}

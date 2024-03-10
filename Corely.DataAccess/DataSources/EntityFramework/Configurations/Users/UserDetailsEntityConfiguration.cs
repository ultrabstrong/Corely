using Corely.DataAccess.Connections;
using Corely.Domain.Constants.Users;
using Corely.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.DataSources.EntityFramework.Configurations.Users
{
    internal class UserDetailsEntityConfiguration : EntityConfigurationBase<UserDetailsEntity>
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

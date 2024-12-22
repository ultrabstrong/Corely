using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Roles.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.IAM.Roles.Entities;

internal sealed class RoleEntityConfiguration : EntityConfigurationBase<RoleEntity>
{
    public RoleEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
    {
    }
    protected override void ConfigureInternal(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.Property(e => e.RoleName)
            .IsRequired()
            .HasMaxLength(RoleConstants.ROLE_NAME_MAX_LENGTH);
    }
}

using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Permissions.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.IAM.Permissions.Entities;
internal sealed class PermissionEntityConfiguration : EntityConfigurationBase<PermissionEntity>
{
    public PermissionEntityConfiguration(IEFDbTypes eFDbTypes) : base(eFDbTypes)
    {
    }

    protected override void ConfigureInternal(EntityTypeBuilder<PermissionEntity> builder)
    {
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(PermissionConstants.PERMISSION_NAME_MAX_LENGTH);

        builder.HasIndex(e => new { e.AccountId, e.Name })
            .IsUnique();
    }
}

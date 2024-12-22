using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Roles.Constants;
using Microsoft.EntityFrameworkCore;
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

        builder.HasMany(e => e.Users)
            .WithMany(e => e.Roles)
            .UsingEntity(j => j.ToTable("UserRoles"));

        builder.HasMany(e => e.Groups)
            .WithMany(e => e.Roles)
            .UsingEntity(j => j.ToTable("GroupRoles"));
    }
}

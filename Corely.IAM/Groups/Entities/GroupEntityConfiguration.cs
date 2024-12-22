using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM.Groups.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.IAM.Groups.Entities;

internal sealed class GroupEntityConfiguration : EntityConfigurationBase<GroupEntity>
{
    public GroupEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
    {
    }

    protected override void ConfigureInternal(EntityTypeBuilder<GroupEntity> builder)
    {
        builder.Property(e => e.GroupName)
            .IsRequired()
            .HasMaxLength(GroupConstants.GROUP_NAME_MAX_LENGTH);

        builder.HasMany(e => e.Roles)
            .WithMany(e => e.Groups)
            .UsingEntity(j => j.ToTable("GroupRoles"));
    }
}

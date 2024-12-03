using Corely.DataAccess.EntityFramework.Configurations;
using Corely.DataAccess.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.EntityFramework;

public abstract class EntityConfigurationBase<T>
    : IEntityTypeConfiguration<T>
    where T : class
{
    protected readonly IEFDbTypes EFDbTypes;

    protected EntityConfigurationBase(IEFDbTypes efDbTypes)
    {
        EFDbTypes = efDbTypes;
    }

    public void Configure(EntityTypeBuilder<T> builder)
    {
        if (typeof(T).Name.EndsWith("Entity"))
        {
            string tableName = typeof(T).Name.Replace("Entity", string.Empty);
            if (!tableName.EndsWith('s'))
            {
                tableName += "s";
            }
            builder.ToTable(tableName);
        }

        if (typeof(IHasIdPk).IsAssignableFrom(typeof(T)))
        {
            builder.HasKey(e => ((IHasIdPk)e).Id);
            builder.Property(e => ((IHasIdPk)e).Id)
                .ValueGeneratedOnAdd();
        }

        if (typeof(IHasCreatedUtc).IsAssignableFrom(typeof(T)))
        {
            builder.Property(e => ((IHasCreatedUtc)e).CreatedUtc)
                .HasColumnType(EFDbTypes.UTCDateColumnType)
                .HasDefaultValueSql(EFDbTypes.UTCDateColumnDefaultValue)
                .IsRequired();
        }

        if (typeof(IHasModifiedUtc).IsAssignableFrom(typeof(T)))
        {
            builder.Property(e => ((IHasModifiedUtc)e).ModifiedUtc)
                .HasColumnType(EFDbTypes.UTCDateColumnType)
                .HasDefaultValueSql(EFDbTypes.UTCDateColumnDefaultValue)
                .IsRequired();
        }

        ConfigureInternal(builder);
    }

    protected abstract void ConfigureInternal(EntityTypeBuilder<T> builder);
}

using Corely.DataAccess.EntityFramework.Configurations;
using Corely.DataAccess.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.Extensions;


public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<TEntity> ConfigureTable<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class
    {
        var tableName = typeof(TEntity).Name;
        if (tableName.EndsWith("Entity"))
        {
            tableName = tableName.Replace("Entity", string.Empty);
        }
        if (!tableName.EndsWith('s'))
        {
            tableName += "s";
        }
        builder.ToTable(tableName);
        return builder;
    }

    public static EntityTypeBuilder<TEntity> ConfigureIdPk<TEntity, TKey>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IHasIdPk<TKey>
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();
        return builder;
    }

    public static EntityTypeBuilder<TEntity> ConfigureCreatedUtc<TEntity>(this EntityTypeBuilder<TEntity> builder, IEFDbTypes efDbTypes)
        where TEntity : class
    {
        if (typeof(IHasCreatedUtc).IsAssignableFrom(typeof(TEntity)))
        {
            builder.Property(e => ((IHasCreatedUtc)e).CreatedUtc)
                .HasColumnType(efDbTypes.UTCDateColumnType)
                .HasDefaultValueSql(efDbTypes.UTCDateColumnDefaultValue)
                .IsRequired();
        }

        return builder;
    }

    public static EntityTypeBuilder<TEntity> ConfigureModifiedUtc<TEntity>(this EntityTypeBuilder<TEntity> builder, IEFDbTypes efDbTypes)
        where TEntity : class
    {
        if (typeof(IHasModifiedUtc).IsAssignableFrom(typeof(TEntity)))
        {
            builder.Property(e => ((IHasModifiedUtc)e).ModifiedUtc)
                .HasColumnType(efDbTypes.UTCDateColumnType);
        }

        return builder;
    }
}

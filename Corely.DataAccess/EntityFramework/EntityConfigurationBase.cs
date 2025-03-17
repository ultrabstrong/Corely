using Corely.DataAccess.EntityFramework.Configurations;
using Corely.DataAccess.Extensions;
using Corely.DataAccess.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.EntityFramework;

public abstract class EntityConfigurationBase<TEntity, TKey>
    : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IHasIdPk<TKey>
{
    protected readonly IEFDbTypes EFDbTypes;
    protected EntityConfigurationBase(IEFDbTypes efDbTypes)
    {
        EFDbTypes = efDbTypes;
    }
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder = builder
            .ConfigureTable()
            .ConfigureIdPk<TEntity, TKey>()
            .ConfigureCreatedUtc(EFDbTypes)
            .ConfigureModifiedUtc(EFDbTypes);
        ConfigureInternal(builder);
    }
    protected abstract void ConfigureInternal(EntityTypeBuilder<TEntity> builder);
}

public abstract class EntityConfigurationBase<TEntity>
    : IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    protected readonly IEFDbTypes EFDbTypes;

    protected EntityConfigurationBase(IEFDbTypes efDbTypes)
    {
        EFDbTypes = efDbTypes;
    }

    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder = builder
            .ConfigureTable()
            .ConfigureCreatedUtc(EFDbTypes)
            .ConfigureModifiedUtc(EFDbTypes);

        ConfigureInternal(builder);
    }

    protected abstract void ConfigureInternal(EntityTypeBuilder<TEntity> builder);
}

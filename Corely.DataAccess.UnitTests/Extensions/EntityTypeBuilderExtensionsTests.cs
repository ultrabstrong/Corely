using Corely.DataAccess.Extensions;
using Corely.DataAccess.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Corely.DataAccess.UnitTests.Extensions;

public class EntityTypeBuilderExtensionsTests
{
    private class FixtureEntity
    {
        public int Id { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? ModifiedUtc { get; set; }
    }

    private readonly EFDbTypesFixture _efDbTypes = new();


    [Fact]
    public void ConfigureTable_SetsTableName()
    {
        var modelBuilder = new ModelBuilder();
        var entityBuilder = modelBuilder.Entity<EntityFixture>();

        Assert.Equal(entityBuilder, entityBuilder.ConfigureTable());

        var entityType = modelBuilder.Model.FindEntityType(typeof(EntityFixture));
        Assert.NotNull(entityType);

        var tableName = entityType.GetTableName();
        Assert.NotNull(tableName);
        Assert.Equal("EntityFixtures", tableName);
    }

    [Fact]
    public void ConfigureTable_ShouldRemoveEntitySuffix()
    {
        var modelBuilder = new ModelBuilder();
        var entityBuilder = modelBuilder.Entity<FixtureEntity>();

        Assert.Equal(entityBuilder, entityBuilder.ConfigureTable());

        var entityType = modelBuilder.Model.FindEntityType(typeof(FixtureEntity));
        Assert.NotNull(entityType);

        var tableName = entityType.GetTableName();
        Assert.NotNull(tableName);
        Assert.Equal("Fixtures", tableName);
    }

    [Fact]
    public void ConfigureIdPk_SetsPrimaryKey()
    {
        var modelBuilder = new ModelBuilder();
        var entityBuilder = modelBuilder.Entity<EntityFixture>();

        Assert.Equal(entityBuilder, entityBuilder.ConfigureIdPk<EntityFixture, int>());

        var entityType = modelBuilder.Model.FindEntityType(typeof(EntityFixture));
        Assert.NotNull(entityType);

        var primaryKey = entityType.FindPrimaryKey();
        Assert.NotNull(primaryKey);
        Assert.Equal(nameof(EntityFixture.Id), primaryKey.Properties.Single().Name);

        var idProperty = entityType.FindProperty(nameof(EntityFixture.Id));
        Assert.NotNull(idProperty);
        Assert.Equal(ValueGenerated.OnAdd, idProperty.ValueGenerated);
    }

    [Fact]
    public void ConfigureCreatedUtc_SetsCreatedUtcProperty()
    {
        var modelBuilder = new ModelBuilder();
        var entityBuilder = modelBuilder.Entity<EntityFixture>();

        Assert.Equal(entityBuilder, entityBuilder.ConfigureCreatedUtc(_efDbTypes));

        var entityType = modelBuilder.Model.FindEntityType(typeof(EntityFixture));
        Assert.NotNull(entityType);

        var createdUtcProperty = entityType.FindProperty(nameof(EntityFixture.CreatedUtc));
        Assert.NotNull(createdUtcProperty);
        Assert.Equal(_efDbTypes.UTCDateColumnType, createdUtcProperty.GetColumnType());
        Assert.Equal(_efDbTypes.UTCDateColumnDefaultValue, createdUtcProperty.GetDefaultValueSql());
        Assert.False(createdUtcProperty.IsNullable);
    }

    [Fact]
    public void ConfigureCreatedUtc_DoesNotSetCreatedUtcProperty()
    {
        var modelBuilder = new ModelBuilder();
        var entityBuilder = modelBuilder.Entity<FixtureEntity>();

        Assert.Equal(entityBuilder, entityBuilder.ConfigureCreatedUtc(_efDbTypes));

        var entityType = modelBuilder.Model.FindEntityType(typeof(FixtureEntity));
        Assert.NotNull(entityType);

        var createdUtcProperty = entityType.FindProperty(nameof(FixtureEntity.CreatedUtc));
        Assert.Null(createdUtcProperty);
    }

    [Fact]
    public void ConfigureModifiedUtc_SetsModifiedUtcProperty()
    {
        var modelBuilder = new ModelBuilder();
        var entityBuilder = modelBuilder.Entity<EntityFixture>();

        Assert.Equal(entityBuilder, entityBuilder.ConfigureModifiedUtc(_efDbTypes));

        var entityType = modelBuilder.Model.FindEntityType(typeof(EntityFixture));
        Assert.NotNull(entityType);

        var modifiedUtcProperty = entityType.FindProperty(nameof(EntityFixture.ModifiedUtc));
        Assert.NotNull(modifiedUtcProperty);
        Assert.Equal(_efDbTypes.UTCDateColumnType, modifiedUtcProperty.GetColumnType());
        Assert.Null(modifiedUtcProperty.GetDefaultValueSql());
        Assert.True(modifiedUtcProperty.IsNullable);
    }

    [Fact]
    public void ConfigureModifiedUtc_DoesNotSetModifiedUtcProperty()
    {
        var modelBuilder = new ModelBuilder();
        var entityBuilder = modelBuilder.Entity<FixtureEntity>();

        Assert.Equal(entityBuilder, entityBuilder.ConfigureModifiedUtc(_efDbTypes));

        var entityType = modelBuilder.Model.FindEntityType(typeof(FixtureEntity));
        Assert.NotNull(entityType);

        var modifiedUtcProperty = entityType.FindProperty(nameof(FixtureEntity.ModifiedUtc));
        Assert.Null(modifiedUtcProperty);
    }
}

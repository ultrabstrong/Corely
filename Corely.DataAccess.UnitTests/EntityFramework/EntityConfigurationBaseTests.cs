using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.EntityFramework.Configurations;
using Corely.DataAccess.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Corely.DataAccess.UnitTests.EntityFramework;

public class EntityConfigurationBaseTests
{
    private class TestEntityConfiguration : EntityConfigurationBase<EntityFixture>
    {
        public bool IsEFDbTypesSet => EFDbTypes != null;
        public bool ConfigureInternalCalled { get; private set; }

        public TestEntityConfiguration(IEFDbTypes efDbTypes) : base(efDbTypes)
        {
        }

        protected override void ConfigureInternal(EntityTypeBuilder<EntityFixture> builder)
        {
            ConfigureInternalCalled = true;
        }
    }

    private class TestEntityConfigurationWithKey : EntityConfigurationBase<EntityFixture, int>
    {
        public bool IsEFDbTypesSet => EFDbTypes != null;
        public bool ConfigureInternalCalled { get; private set; }

        public TestEntityConfigurationWithKey(IEFDbTypes efDbTypes) : base(efDbTypes)
        {
        }

        protected override void ConfigureInternal(EntityTypeBuilder<EntityFixture> builder)
        {
            ConfigureInternalCalled = true;
        }
    }

    private readonly EFDbTypesFixture _efDbTypes = new();

    [Fact]
    public void Configure_CallsConfigureInternal()
    {
        var modelBuilder = new ModelBuilder();
        var entityBuilder = modelBuilder.Entity<EntityFixture>();
        var entityConfiguration = new TestEntityConfiguration(_efDbTypes);

        entityConfiguration.Configure(entityBuilder);

        Assert.True(entityConfiguration.IsEFDbTypesSet);
        Assert.True(entityConfiguration.ConfigureInternalCalled);

        var entityType = modelBuilder.Model.FindEntityType(typeof(EntityFixture));
        Assert.NotNull(entityType);
        Assert.NotNull(entityType.GetTableName());
        Assert.Null(entityType.FindProperty(nameof(EntityFixture.Id)));
        Assert.NotNull(entityType.FindProperty(nameof(EntityFixture.CreatedUtc)));
        Assert.NotNull(entityType.FindProperty(nameof(EntityFixture.ModifiedUtc)));
    }

    [Fact]
    public void Configure_CallsConfigureInternalWithKey()
    {
        var modelBuilder = new ModelBuilder();
        var entityBuilder = modelBuilder.Entity<EntityFixture>();
        var entityConfiguration = new TestEntityConfigurationWithKey(_efDbTypes);

        entityConfiguration.Configure(entityBuilder);

        Assert.True(entityConfiguration.IsEFDbTypesSet);
        Assert.True(entityConfiguration.ConfigureInternalCalled);

        var entityType = modelBuilder.Model.FindEntityType(typeof(EntityFixture));
        Assert.NotNull(entityType);
        Assert.NotNull(entityType.GetTableName());
        Assert.NotNull(entityType.FindProperty(nameof(EntityFixture.Id)));
        Assert.NotNull(entityType.FindProperty(nameof(EntityFixture.CreatedUtc)));
        Assert.NotNull(entityType.FindProperty(nameof(EntityFixture.ModifiedUtc)));
    }
}

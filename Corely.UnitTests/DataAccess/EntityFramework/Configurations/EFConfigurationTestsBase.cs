using Corely.DataAccess.EntityFramework.Configurations;

namespace Corely.UnitTests.DataAccess.EntityFramework.Configurations
{
    public abstract class EFConfigurationTestsBase
    {
        protected abstract IEFConfiguration EFConfiguration { get; }

        [Fact]
        public void GetDbTypes_ReturnsEFDbTypes()
        {
            var dbTypes = EFConfiguration.GetDbTypes();
            Assert.NotNull(dbTypes);

            Assert.NotEmpty(dbTypes.UTCDateColumnType);
            Assert.NotEmpty(dbTypes.UTCDateColumnDefaultValue);
        }
    }
}

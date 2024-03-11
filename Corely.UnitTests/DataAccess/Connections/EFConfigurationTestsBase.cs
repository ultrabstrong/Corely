using Corely.DataAccess.Connections;

namespace Corely.UnitTests.DataAccess.Connections
{
    public abstract class EFConfigurationTestsBase
    {
        protected abstract IEFConfiguration EFConfiguration { get; }

        protected static void VerifyDbTypes(IEFDbTypes dbTypes)
        {
        }

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

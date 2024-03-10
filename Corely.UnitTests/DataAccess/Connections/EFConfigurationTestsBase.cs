using Corely.DataAccess.Connections;

namespace Corely.UnitTests.DataAccess.Connections
{
    public abstract class EFConfigurationTestsBase
    {
        protected static void VerifyDbTypes(IEFDbTypes dbTypes)
        {
            Assert.NotNull(dbTypes);
            Assert.IsAssignableFrom<IEFDbTypes>(dbTypes);

            Assert.NotEmpty(dbTypes.UTCDateColumnType);
            Assert.NotEmpty(dbTypes.UTCDateColumnDefaultValue);
        }
    }
}

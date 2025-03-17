using Corely.DataAccess.EntityFramework.Configurations;

namespace Corely.DataAccess.UnitTests.Fixtures;

internal class EFDbTypesFixture : IEFDbTypes
{
    public string UTCDateColumnType => nameof(UTCDateColumnType);

    public string UTCDateColumnDefaultValue => nameof(UTCDateColumnDefaultValue);
}

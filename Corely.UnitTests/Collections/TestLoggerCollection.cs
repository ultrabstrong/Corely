using Corely.UnitTests.Fixtures;

namespace Corely.UnitTests.Collections
{
    [CollectionDefinition(nameof(CollectionNames.SerilogCollection))]
    public class TestLoggerCollection : ICollectionFixture<TestLoggerFixture>
    {
        // This class has no code and is never instantiated. 
        // Its purpose is to define a collection for sharing the TestLoggerFixture.
    }
}

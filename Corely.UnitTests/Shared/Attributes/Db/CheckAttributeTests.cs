using Corely.Shared.Attributes.Db;

namespace Corely.UnitTests.Shared.Attributes.Db
{
    public class CheckAttributeTests
    {
        [Fact]
        public void CheckConstructor_ShouldThrowArgumentNullException_WhenExpressionIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CheckAttribute(null));
            Assert.Throws<ArgumentNullException>(() => new CheckAttribute(null, true));
            Assert.Throws<ArgumentNullException>(() => new CheckAttribute(null, true)
            {
                InitiallyDeferred = true
            });
        }

        [Fact]
        public void InitiallyDeferred_ShouldThrowArgumentException_WhenDeferrableIsNotTrue()
        {
            Assert.Throws<ArgumentException>(() => new CheckAttribute("1 = 1", false)
            {
                InitiallyDeferred = true
            });
        }
    }
}

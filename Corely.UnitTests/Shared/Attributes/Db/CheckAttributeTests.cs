using Corely.Shared.Attributes.Db;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Shared.Attributes.Db
{
    public class CheckAttributeTests
    {
        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        public void CheckConstructor_ShouldThrowException_WhenExpressionIsInvalid(string value)
        {
            if (value == null)
            {
                Assert.Throws<ArgumentNullException>(() => new CheckAttribute(value));
                Assert.Throws<ArgumentNullException>(() => new CheckAttribute(value, true));
                Assert.Throws<ArgumentNullException>(() => new CheckAttribute(value, true)
                {
                    InitiallyDeferred = true
                });
            }
            else
            {
                Assert.Throws<ArgumentException>(() => new CheckAttribute(value));
                Assert.Throws<ArgumentException>(() => new CheckAttribute(value, true));
                Assert.Throws<ArgumentException>(() => new CheckAttribute(value, true)
                {
                    InitiallyDeferred = true
                });
            }
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

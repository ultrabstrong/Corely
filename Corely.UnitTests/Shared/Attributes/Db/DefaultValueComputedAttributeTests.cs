using Corely.Shared.Attributes.Db;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Shared.Attributes.Db
{
    public class DefaultValueComputedAttributeTests
    {
        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        public void DefaultValueComputedConstructor_ThrowsException_WhenExpressionIsInvalid(string expression)
        {
            void act() => new DefaultValueComputedAttribute(expression);

            if (expression == null)
                Assert.Throws<ArgumentNullException>(act);
            else
                Assert.Throws<ArgumentException>(act);
        }
    }
}

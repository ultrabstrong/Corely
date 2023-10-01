using Corely.Shared.Attributes.Db;

namespace Corely.UnitTests.Shared.Attributes.Db
{
    public class AutoIncrementAttributeTests
    {
        [Fact]
        public void AutoIncrementConstructor_ShouldThrowException_WhenStartWithIsLessThanZero()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new AutoIncrementAttribute(1, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new AutoIncrementAttribute(1)
            {
                StartWith = -1
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => new AutoIncrementAttribute()
            {
                StartWith = -1
            });
        }

        [Fact]
        public void AutoIncrementConstructor_ShouldThrowException_WhenIncrementByIsLessThanOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new AutoIncrementAttribute(0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new AutoIncrementAttribute(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new AutoIncrementAttribute()
            {
                IncrementBy = 0
            });
        }

        [Fact]
        public void AutoIncrementConstructor_ShouldSucceed_WithMinimumValues()
        {
            var attr = new AutoIncrementAttribute(0, 1);
            Assert.Equal(0, attr.StartWith);
            Assert.Equal(1, attr.IncrementBy);

            attr = new AutoIncrementAttribute(1);
            Assert.Null(attr.StartWith);
            Assert.Equal(1, attr.IncrementBy);

            attr = new AutoIncrementAttribute()
            {
                StartWith = 0,
                IncrementBy = 1
            };
            Assert.Equal(0, attr.StartWith);
            Assert.Equal(1, attr.IncrementBy);


            attr = new AutoIncrementAttribute()
            {
                StartWith = 0
            };
            Assert.Equal(0, attr.StartWith);
            Assert.Null(attr.IncrementBy);

            attr = new AutoIncrementAttribute()
            {
                StartWith = 0,
                IncrementBy = null
            };
            Assert.Equal(0, attr.StartWith);
            Assert.Null(attr.IncrementBy);


            attr = new AutoIncrementAttribute()
            {
                IncrementBy = 1
            };
            Assert.Null(attr.StartWith);
            Assert.Equal(1, attr.IncrementBy);

            attr = new AutoIncrementAttribute()
            {
                StartWith = null,
                IncrementBy = 1
            };
            Assert.Null(attr.StartWith);
            Assert.Equal(1, attr.IncrementBy);


            attr = new AutoIncrementAttribute();
            Assert.Null(attr.StartWith);
            Assert.Null(attr.IncrementBy);
        }
    }
}

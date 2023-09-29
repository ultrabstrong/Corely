using Corely.Shared.Attributes.Db;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Shared.Attributes.Db
{
    public class ColumnPlacementAttributeTests
    {
        [Fact]
        public void Properties_ShouldThrowArgumentException_WhenAfterColumnAndBeforeColumnAreSame()
        {
            Assert.Throws<ArgumentException>(() => new ColumnPlacementAttribute("column", "column"));
            Assert.Throws<ArgumentException>(() => new ColumnPlacementAttribute("column")
            {
                BeforeColumn = "column"
            });
            Assert.Throws<ArgumentException>(() => new ColumnPlacementAttribute()
            {
                AfterColumn = "column",
                BeforeColumn = "column"
            });
            Assert.Throws<ArgumentException>(() => new ColumnPlacementAttribute()
            {
                BeforeColumn = "column",
                AfterColumn = "column"
            });
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        public void AfterColumn_ShouldThrowArgumentException_WhenNullOrWhiteSpace(string afterColumn)
        {
            void act() => new ColumnPlacementAttribute(afterColumn);
            if (afterColumn == null)
                Assert.Throws<ArgumentNullException>(act);
            else
                Assert.Throws<ArgumentException>(act);


            void act2() => new ColumnPlacementAttribute()
            {
                AfterColumn = afterColumn
            };
            if (afterColumn == null)
                Assert.Throws<ArgumentNullException>(act2);
            else
                Assert.Throws<ArgumentException>(act2);


            void act3() => new ColumnPlacementAttribute(afterColumn, "column");
            if (afterColumn == null)
                Assert.Throws<ArgumentNullException>(act3);
            else
                Assert.Throws<ArgumentException>(act3);
        }


        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        public void BeforeColumn_ShouldThrowArgumentException_WhenNullOrWhiteSpace(string beforeColumn)
        {
            void act() => new ColumnPlacementAttribute()
            {
                BeforeColumn = beforeColumn
            };
            if (beforeColumn == null)
                Assert.Throws<ArgumentNullException>(act);
            else
                Assert.Throws<ArgumentException>(act);


            void act2() => new ColumnPlacementAttribute("column", beforeColumn);
            if (beforeColumn == null)
                Assert.Throws<ArgumentNullException>(act2);
            else
                Assert.Throws<ArgumentException>(act2);
        }
    }
}

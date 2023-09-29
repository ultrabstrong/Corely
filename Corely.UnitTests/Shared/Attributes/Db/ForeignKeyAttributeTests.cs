using Corely.Shared.Attributes.Db;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Shared.Attributes.Db
{
    public class ForeignKeyAttributeTests
    {
        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        public void ForeignKeyConstructor_ThrowsException_WhenColumnsAreInvalid(string column)
        {
            void act() => new ForeignKeyAttribute("table", column);

            if (column == null)
                Assert.Throws<ArgumentNullException>(act);
            else
                Assert.Throws<ArgumentException>(act);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        public void ForeignKeyConstructor_ThrowsException_WhenTableIsInvalid(string table)
        {
            void act() => new ForeignKeyAttribute(table, "column");

            if (table == null)
                Assert.Throws<ArgumentNullException>(act);
            else
                Assert.Throws<ArgumentException>(act);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        public void ForeignKeyConstructor_ThrowsException_WhenSchemaIsInvalid(string schema)
        {
            void act() => new ForeignKeyAttribute(schema, "table", "column");

            if (schema == null)
                Assert.Throws<ArgumentNullException>(act);
            else
                Assert.Throws<ArgumentException>(act);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        public void ForeignKeyConstructor_ThrowsException_WhenCustomSqlIsInvalid(string customSql)
        {
            void act() => new ForeignKeyAttribute(customSql);

            if (customSql == null)
                Assert.Throws<ArgumentNullException>(act);
            else
                Assert.Throws<ArgumentException>(act);
        }
    }
}

using AutoFixture;
using Corely.Common.Extensions;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Common.Extensions
{
    public class ThrowIfNullExtensionsTests
    {
        private readonly Fixture _fixture = new();

        private class TestClass { }

        [Fact]
        public void ThrowIfNull_ThrowsException_WithNullString()
        {
            string? value = null;

            var ex = Record.Exception(() => value.ThrowIfNull(nameof(value)));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void ThrowIfAnyNull_ThrowsException_WithNullString()
        {
            string[] values = [_fixture.Create<string>(), null];

            var ex = Record.Exception(() => values.ThrowIfAnyNull(nameof(values)));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void ThrowIfNull_ThrowsException_WithNullObject()
        {
            TestClass? value = null;

            var ex = Record.Exception(() => value.ThrowIfNull(nameof(value)));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void ThrowIfAnyNull_ThrowsException_WithNullObject()
        {
            TestClass?[] values = [_fixture.Create<TestClass>(), null];

            var ex = Record.Exception(() => values.ThrowIfAnyNull(nameof(values)));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        public void ThrowIfNullOrWhitespace_ThrowsException_WithInvalidValue(string value)
        {
            var ex = Record.Exception(() => value.ThrowIfNullOrWhiteSpace(nameof(value)));

            Assert.NotNull(ex);
            if (value == null)
            {
                Assert.IsType<ArgumentNullException>(ex);
            }
            else
            {
                Assert.IsType<ArgumentException>(ex);
            }
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        public void ThrowIfAnyNullOrWhitespace_ThrowsException_WithInvalidValue(string value)
        {
            string[] values = [_fixture.Create<string>(), value];

            var ex = Record.Exception(() => values.ThrowIfAnyNullOrWhiteSpace(nameof(values)));

            Assert.NotNull(ex);
            if (value == null)
            {
                Assert.IsType<ArgumentNullException>(ex);
            }
            else
            {
                Assert.IsType<ArgumentException>(ex);
            }
        }

        [Theory]
        [ClassData(typeof(NullAndEmpty))]
        public void ThrowIfNullOrEmpty_ThrowsException_WithInvalidValue(string value)
        {
            var ex = Record.Exception(() => value.ThrowIfNullOrEmpty(nameof(value)));

            Assert.NotNull(ex);
            if (value == null)
            {
                Assert.IsType<ArgumentNullException>(ex);
            }
            else
            {
                Assert.IsType<ArgumentException>(ex);
            }
        }

        [Theory]
        [ClassData(typeof(NullAndEmpty))]
        public void ThrowIfAnyNullOrEmpty_ThrowsException_WithInvalidValue(string value)
        {
            string[] values = [_fixture.Create<string>(), value];

            var ex = Record.Exception(() => values.ThrowIfAnyNullOrEmpty(nameof(values)));

            Assert.NotNull(ex);
            if (value == null)
            {
                Assert.IsType<ArgumentNullException>(ex);
            }
            else
            {
                Assert.IsType<ArgumentException>(ex);
            }
        }

        [Fact]
        public void ThrowIfNull_ReturnsObject_WithValidObject()
        {
            var value = _fixture.Create<TestClass>();

            var result = value.ThrowIfNull(nameof(value));

            Assert.Equal(value, result);
        }

        [Fact]
        public void ThrowIfAnyNull_ReturnsObject_WithValidObject()
        {
            var values = new[] { _fixture.Create<TestClass>(), _fixture.Create<TestClass>() };

            var result = values.ThrowIfAnyNull(nameof(values));

            Assert.Equal(values, result);
        }

        [Fact]
        public void ThrowIfNullOrWhitespace_ReturnsString_WithValidString()
        {
            var value = _fixture.Create<string>();

            var result = value.ThrowIfNullOrWhiteSpace(nameof(value));

            Assert.Equal(value, result);
        }

        [Fact]
        public void ThrowIfAnyNullOrWhitespace_ReturnsString_WithValidString()
        {
            var values = new[] { _fixture.Create<string>(), _fixture.Create<string>() };

            var result = values.ThrowIfAnyNullOrWhiteSpace(nameof(values));

            Assert.Equal(values, result);
        }

        [Fact]
        public void ThrowIfNullOrEmpty_ReturnsString_WithValidString()
        {
            var value = _fixture.Create<string>();

            var result = value.ThrowIfNullOrEmpty(nameof(value));

            Assert.Equal(value, result);
        }

        [Fact]
        public void ThrowIfAnyNullOrEmpty_ReturnsString_WithValidString()
        {
            var values = new[] { _fixture.Create<string>(), _fixture.Create<string>() };

            var result = values.ThrowIfAnyNullOrEmpty(nameof(values));

            Assert.Equal(values, result);
        }
    }
}

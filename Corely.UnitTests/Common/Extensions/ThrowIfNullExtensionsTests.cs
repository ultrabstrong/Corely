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
        public void ThrowIfNull_ShouldThrowException_WithNullString()
        {
            string? value = null;

            void act() => value.ThrowIfNull(nameof(value));

            Assert.Throws<ArgumentNullException>(nameof(value), act);
        }

        [Fact]
        public void ThrowIfAnyNull_ShouldThrowException_WithNullString()
        {
            string[] values = new[] { _fixture.Create<string>(), null };

            void act() => values.ThrowIfAnyNull(nameof(values));

            Assert.Throws<ArgumentNullException>(nameof(values), act);
        }

        [Fact]
        public void ThrowIfNull_ShouldThrowException_WithNullObject()
        {
            TestClass? value = null;

            void act() => value.ThrowIfNull(nameof(value));

            Assert.Throws<ArgumentNullException>(nameof(value), act);
        }

        [Fact]
        public void ThrowIfAnyNull_ShouldThrowException_WithNullObject()
        {
            TestClass[] values = new[] { _fixture.Create<TestClass>(), null };

            void act() => values.ThrowIfAnyNull(nameof(values));

            Assert.Throws<ArgumentNullException>(nameof(values), act);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        public void ThrowIfNullOrWhitespace_ShouldThrowException_WithInvalidValue(string value)
        {
            void act() => value.ThrowIfNullOrWhiteSpace(nameof(value));

            if (value == null)
            {
                Assert.Throws<ArgumentNullException>(nameof(value), act);
            }
            else
            {
                Assert.Throws<ArgumentException>(nameof(value), act);
            }
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        public void ThrowIfAnyNullOrWhitespace_ShouldThrowException_WithInvalidValue(string value)
        {
            string[] values = [_fixture.Create<string>(), value];

            void act() => values.ThrowIfAnyNullOrWhiteSpace(nameof(values));

            if (value == null)
            {
                Assert.Throws<ArgumentNullException>(nameof(values), act);
            }
            else
            {
                Assert.Throws<ArgumentException>(nameof(values), act);
            }
        }

        [Theory]
        [ClassData(typeof(NullAndEmpty))]
        public void ThrowIfNullOrEmpty_ShouldThrowException_WithInvalidValue(string value)
        {
            void act() => value.ThrowIfNullOrEmpty(nameof(value));

            if (value == null)
            {
                Assert.Throws<ArgumentNullException>(nameof(value), act);
            }
            else
            {
                Assert.Throws<ArgumentException>(nameof(value), act);
            }
        }

        [Theory]
        [ClassData(typeof(NullAndEmpty))]
        public void ThrowIfAnyNullOrEmpty_ShouldThrowException_WithInvalidValue(string value)
        {
            string[] values = [_fixture.Create<string>(), value];

            void act() => values.ThrowIfAnyNullOrEmpty(nameof(values));

            if (value == null)
            {
                Assert.Throws<ArgumentNullException>(nameof(values), act);
            }
            else
            {
                Assert.Throws<ArgumentException>(nameof(values), act);
            }
        }

        [Fact]
        public void ThrowIfNull_ShouldReturnObject_WithValidObject()
        {
            var value = _fixture.Create<TestClass>();

            var result = value.ThrowIfNull(nameof(value));

            Assert.Equal(value, result);
        }

        [Fact]
        public void ThrowIfAnyNull_ShouldReturnObject_WithValidObject()
        {
            var values = new[] { _fixture.Create<TestClass>(), _fixture.Create<TestClass>() };

            var result = values.ThrowIfAnyNull(nameof(values));

            Assert.Equal(values, result);
        }

        [Fact]
        public void ThrowIfNullOrWhitespace_ShouldReturnString_WithValidString()
        {
            var value = _fixture.Create<string>();

            var result = value.ThrowIfNullOrWhiteSpace(nameof(value));

            Assert.Equal(value, result);
        }

        [Fact]
        public void ThrowIfAnyNullOrWhitespace_ShouldReturnString_WithValidString()
        {
            var values = new[] { _fixture.Create<string>(), _fixture.Create<string>() };

            var result = values.ThrowIfAnyNullOrWhiteSpace(nameof(values));

            Assert.Equal(values, result);
        }

        [Fact]
        public void ThrowIfNullOrEmpty_ShouldReturnString_WithValidString()
        {
            var value = _fixture.Create<string>();

            var result = value.ThrowIfNullOrEmpty(nameof(value));

            Assert.Equal(value, result);
        }

        [Fact]
        public void ThrowIfAnyNullOrEmpty_ShouldReturnString_WithValidString()
        {
            var values = new[] { _fixture.Create<string>(), _fixture.Create<string>() };

            var result = values.ThrowIfAnyNullOrEmpty(nameof(values));

            Assert.Equal(values, result);
        }
    }
}

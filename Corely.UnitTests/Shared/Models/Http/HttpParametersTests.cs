﻿using Corely.Shared.Models.Http;

namespace Corely.UnitTests.Shared.Models.Http
{
    public class HttpParametersTests
    {
        private readonly HttpParametersBase _httpParameters;

        public HttpParametersTests()
        {
            _httpParameters = new HttpParameters();
        }

        [Fact]
        public void HttpParametersBaseConstructor_ShouldSetProperties()
        {
            Assert.False(_httpParameters.HasParameters());
            Assert.Equal(0, _httpParameters.GetParameterCount());

            Assert.False(_httpParameters.HasTempParameters());
            Assert.Equal(0, _httpParameters.GetTempParameterCount());
        }

        [Fact]
        public void HttpParametersStringConstructor_ShouldSetProperties()
        {
            var httpParameters = new HttpParameters("key", "value");

            Assert.True(httpParameters.HasParameters());
            Assert.Equal(1, httpParameters.GetParameterCount());
            Assert.Equal("value", httpParameters.GetParameterValue("key"));

            Assert.False(httpParameters.HasTempParameters());
            Assert.Equal(0, httpParameters.GetTempParameterCount());
        }

        [Fact]
        public void HttpParametersDictionaryConstructors_ShouldSetProperties()
        {
            var parameters = new Dictionary<string, string>() { { "key1", "value1" } };
            var tempParameters = new Dictionary<string, string>() { { "key2", "value2" } };

            var httpParameters = new HttpParameters(parameters, tempParameters);

            Assert.True(httpParameters.HasParameters());
            Assert.Equal(1, httpParameters.GetParameterCount());
            Assert.Equal("value1", httpParameters.GetParameterValue("key1"));

            Assert.True(httpParameters.HasTempParameters());
            Assert.Equal(1, httpParameters.GetTempParameterCount());
            Assert.Equal("value2", httpParameters.GetTempParameterValue("key2"));
        }

        [Fact]
        public void CreateParameters_ShouldReturnEmptyString_WhenNoParametersAreAdded()
        {
            Assert.Equal(string.Empty, _httpParameters.CreateParameters());
        }

        [Theory, MemberData(nameof(CreateParametersInvalidTestData))]
        public void AddParameters_ShouldThrowArgumentNullException_WhenKeyOrValueIsNull(string key, string value)
        {
            Assert.Throws<ArgumentNullException>(() => _httpParameters.AddParameters((key, value)));
        }

        public static IEnumerable<object[]> CreateParametersInvalidTestData()
        {
            yield return new object[] { null, "value" };
            yield return new object[] { "key", null };
            yield return new object[] { null, null };
        }

        [Theory, MemberData(nameof(CreateParametersTestData))]
        public void CreateParameters_ShouldReturnExpectedString_WhenParametersAreAdded((string key, string value) keyValuePairs, string expected)
        {
            _httpParameters.AddParameters(keyValuePairs);

            Assert.Equal(expected, _httpParameters.CreateParameters());
        }

        [Theory, MemberData(nameof(CreateParametersTestData))]
        public void CreateParameters_ShouldReturnExpectedString_WhenTempParametersAreAdded((string key, string value) keyValuePairs, string expected)
        {
            _httpParameters.AddTempParameters(keyValuePairs);

            Assert.Equal(expected, _httpParameters.CreateParameters());
        }

        [Theory, MemberData(nameof(CreateParametersTestData))]
        public void CreateParameters_ShouldReturnExpectedString_WhenBothParametersAreAdded((string key, string value) keyValuePairs, string expected)
        {
            _httpParameters.AddParameters(keyValuePairs);
            _httpParameters.AddTempParameters(keyValuePairs);

            Assert.Equal($"{expected}&{expected}", _httpParameters.CreateParameters());
        }

        public static IEnumerable<object[]> CreateParametersTestData()
        {
            yield return new object[] { ("!@#$%^&*()_+", "+_)(*&^%$#@!"), "%21%40%23%24%25%5E%26%2A%28%29_%2B=%2B_%29%28%2A%26%5E%25%24%23%40%21" };
            yield return new object[] { ("!@#$%^&*()_+", "test"), "%21%40%23%24%25%5E%26%2A%28%29_%2B=test" };
            yield return new object[] { ("test", "!@#$%^&*()_+"), "test=%21%40%23%24%25%5E%26%2A%28%29_%2B" };
            yield return new object[] { ("test", "test"), "test=test" };
            yield return new object[] { ("", "test"), "=test" };
            yield return new object[] { ("test", ""), "test=" };
            yield return new object[] { ("", ""), "=" };
            yield return new object[] { (" ", "test"), "%20=test" };
            yield return new object[] { ("test", " "), "test=%20" };
            yield return new object[] { (" ", " "), "%20=%20" };
        }

        [Fact]
        public void CreateParameters_ShouldChainParameters_WhenMoreThanOneAdded()
        {
            _httpParameters.AddParameters(("key1", "value1"), ("key2", "value2"));

            Assert.Equal("key1=value1&key2=value2", _httpParameters.CreateParameters());
        }

        [Fact]
        public void CreateParameters_ShouldChainTempParameters_WhenMoreThanOneAdded()
        {
            _httpParameters.AddTempParameters(("key1", "value1"), ("key2", "value2"));

            Assert.Equal("key1=value1&key2=value2", _httpParameters.CreateParameters());
        }

        [Fact]
        public void CreateParameters_ShouldChainBothParameters_WhenMoreThanOneAdded()
        {
            _httpParameters.AddParameters(("key1", "value1"), ("key2", "value2"));
            _httpParameters.AddTempParameters(("key3", "value3"), ("key4", "value4"));

            Assert.Equal("key1=value1&key2=value2&key3=value3&key4=value4", _httpParameters.CreateParameters());
        }

        [Fact]
        public void CreateParameters_ShouldClearTempParameters()
        {

            _httpParameters.AddParameters(("key1", "value1"), ("key2", "value2"));
            _httpParameters.AddTempParameters(("key3", "value3"), ("key4", "value4"));

            Assert.True(_httpParameters.HasParameters());
            Assert.Equal(2, _httpParameters.GetParameterCount());
            Assert.True(_httpParameters.HasTempParameters());
            Assert.Equal(2, _httpParameters.GetTempParameterCount());

            Assert.Equal("key1=value1&key2=value2&key3=value3&key4=value4", _httpParameters.CreateParameters());

            Assert.True(_httpParameters.HasParameters());
            Assert.Equal(2, _httpParameters.GetParameterCount());
            Assert.False(_httpParameters.HasTempParameters());
            Assert.Equal(0, _httpParameters.GetTempParameterCount());
        }

        [Fact]
        public void ChangingParameters_ShouldReflectChangedParameters()
        {
            _httpParameters.AddParameters(("key1", "value1"), ("key2", "value2"));

            Assert.True(_httpParameters.HasParameters());
            Assert.Equal(2, _httpParameters.GetParameterCount());
            Assert.Equal("value1", _httpParameters.GetParameterValue("key1"));
            Assert.Equal("value2", _httpParameters.GetParameterValue("key2"));
            Assert.False(_httpParameters.HasTempParameters());
            Assert.Equal(0, _httpParameters.GetTempParameterCount());

            _httpParameters.AddParameters(("key3", "value3"), ("key4", "value4"));

            Assert.True(_httpParameters.HasParameters());
            Assert.Equal(4, _httpParameters.GetParameterCount());
            Assert.Equal("value1", _httpParameters.GetParameterValue("key1"));
            Assert.Equal("value2", _httpParameters.GetParameterValue("key2"));
            Assert.Equal("value3", _httpParameters.GetParameterValue("key3"));
            Assert.Equal("value4", _httpParameters.GetParameterValue("key4"));
            Assert.False(_httpParameters.HasTempParameters());
            Assert.Equal(0, _httpParameters.GetTempParameterCount());

            _httpParameters.RemoveParameters("key1", "key3");
            Assert.True(_httpParameters.HasParameters());
            Assert.Equal(2, _httpParameters.GetParameterCount());
            Assert.Equal("value2", _httpParameters.GetParameterValue("key2"));
            Assert.Equal("value4", _httpParameters.GetParameterValue("key4"));
            Assert.False(_httpParameters.HasTempParameters());
            Assert.Equal(0, _httpParameters.GetTempParameterCount());
        }

        [Fact]
        public void ChangingTempParameters_ShouldReflectChangedTempParameters()
        {
            _httpParameters.AddTempParameters(("key1", "value1"), ("key2", "value2"));

            Assert.False(_httpParameters.HasParameters());
            Assert.Equal(0, _httpParameters.GetParameterCount());
            Assert.True(_httpParameters.HasTempParameters());
            Assert.Equal(2, _httpParameters.GetTempParameterCount());
            Assert.Equal("value1", _httpParameters.GetTempParameterValue("key1"));
            Assert.Equal("value2", _httpParameters.GetTempParameterValue("key2"));

            _httpParameters.AddTempParameters(("key3", "value3"), ("key4", "value4"));

            Assert.False(_httpParameters.HasParameters());
            Assert.Equal(0, _httpParameters.GetParameterCount());
            Assert.True(_httpParameters.HasTempParameters());
            Assert.Equal(4, _httpParameters.GetTempParameterCount());
            Assert.Equal("value1", _httpParameters.GetTempParameterValue("key1"));
            Assert.Equal("value2", _httpParameters.GetTempParameterValue("key2"));
            Assert.Equal("value3", _httpParameters.GetTempParameterValue("key3"));
            Assert.Equal("value4", _httpParameters.GetTempParameterValue("key4"));

            _httpParameters.RemoveTempParameters("key2", "key4");

            Assert.False(_httpParameters.HasParameters());
            Assert.Equal(0, _httpParameters.GetParameterCount());
            Assert.True(_httpParameters.HasTempParameters());
            Assert.Equal(2, _httpParameters.GetTempParameterCount());
            Assert.Equal("value1", _httpParameters.GetTempParameterValue("key1"));
            Assert.Equal("value3", _httpParameters.GetTempParameterValue("key3"));
        }
    }
}

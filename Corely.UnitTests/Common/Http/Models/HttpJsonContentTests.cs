using Corely.Common.Http.Models;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Common.Http.Models;

public class HttpJsonContentTests
{
    private readonly HttpJsonContent _httpJsonContent = new("content");

    [Fact]
    public void HttpFormUrlEncodedContent_IsOfTypeIHttpContent()
    {
        Assert.IsAssignableFrom<IHttpContent<string>>(_httpJsonContent);
    }

    [Fact]
    public void HttpJsonContent_IsOfTypeHttpStringContentBase()
    {
        Assert.IsAssignableFrom<HttpStringContentBase>(_httpJsonContent);
    }

    [Fact]
    public void HttpJsonContent_SetsContent_OnConstruction()
    {
        Assert.Equal("content", _httpJsonContent.Content);
    }

    [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
    public void HttpJsonContent_AllowsEmptyContent(string content)
    {
        var httpJsonContent = new HttpJsonContent(content);
        Assert.Equal(content, httpJsonContent.Content);
    }

    [Theory, MemberData(nameof(HttpJsonContentTestData))]
    public void HttpJsonContent_SerializesContent(object content, string expected)
    {
        var httpJsonContent = new HttpJsonContent(content);
        Assert.Equal(expected, httpJsonContent.Content);
    }

    public static IEnumerable<object[]> HttpJsonContentTestData() =>
    [
        [new { Test = "test" }, "{\"Test\":\"test\"}"],
            [new { Test = 1 }, "{\"Test\":1}"],
            [new { Test = 1.1 }, "{\"Test\":1.1}"],
            [new { Test = true }, "{\"Test\":true}"],
            [new { Test = new { Test = "test" } }, "{\"Test\":{\"Test\":\"test\"}}"],
            [new { Test = new { Test = 1 } }, "{\"Test\":{\"Test\":1}}"],
            [new { Test = new { Test = 1.1 } }, "{\"Test\":{\"Test\":1.1}}"],
            [new { Test = new { Test = true } }, "{\"Test\":{\"Test\":true}}"],
            [new[] { "test" }, "[\"test\"]"],
            [new[] { 1 }, "[1]"],
            [new[] { 1.1 }, "[1.1]"],
            [new[] { true }, "[true]"],
            [new[] { new { Test = "test" } }, "[{\"Test\":\"test\"}]"],
            [new[] { new { Test = 1 } }, "[{\"Test\":1}]"],
            [new[] { new { Test = 1.1 } }, "[{\"Test\":1.1}]"],
            [new[] { new { Test = true } }, "[{\"Test\":true}]"],
            [null, "null"],
            [new { Test = (object?)null }, "{\"Test\":null}"],
            [new { Test = new { Test = (object?)null } }, "{\"Test\":{\"Test\":null}}"],
            [new { Test = new { Test = (object?)null } }, "{\"Test\":{\"Test\":null}}"]
    ];
}

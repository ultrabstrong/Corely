using Corely.Common.File;
using Corely.TestBase.ClassData;
using Moq;

namespace Corely.Common.UnitTests.File;

public class TestableFilePathProvider : FilePathProvider
{
    public override bool DoesFileExist(string filepath)
    {
        return base.DoesFileExist(filepath);
    }
}

public class FilePathProviderTests
{
    private readonly Mock<TestableFilePathProvider> _filePathProviderMock = new() { CallBase = true };
    private bool _doesFileExist;

    private void SetupStandardReturnForDoesFileExist()
    {
        _filePathProviderMock
            .Setup(m => m.DoesFileExist(It.IsAny<string>()))
            .Returns(() => _doesFileExist);
    }


    [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
    public void DoesFileExist_WhenPathIsNullOrWhitespace_ReturnsFalse(string path)
    {
        _doesFileExist = false;
        SetupStandardReturnForDoesFileExist();

        Assert.False(_filePathProviderMock.Object.DoesFileExist(path));
    }

    [Theory]
    [InlineData("C:\\file_that_does_not_exist.txt")]
    public void DoesFileExist_WhenFileDoesNotExist_ReturnsFalse(string path)
    {
        _doesFileExist = false;
        SetupStandardReturnForDoesFileExist();

        Assert.False(_filePathProviderMock.Object.DoesFileExist(path));
    }

    [Theory]
    [InlineData("C:\\file_that_exists.txt")]
    public void DoesFileExist_WhenFileExists_ReturnsTrue(string path)
    {
        _doesFileExist = true;
        SetupStandardReturnForDoesFileExist();

        Assert.True(_filePathProviderMock.Object.DoesFileExist(path));
    }

    [Theory]
    [MemberData(nameof(GetOverwriteProtectedPathTestData), 3)]
    [MemberData(nameof(GetOverwriteProtectedPathTestData), 1)]
    [MemberData(nameof(GetOverwriteProtectedPathTestData), 0)]
    public void GetOverwriteProtectedPath_WhenFileExists_ReturnsOverwriteProtectedPath(int number, string path, string expected)
    {
        SetupDoesFileExistForGetOverwriteProtectedPath(number);

        Assert.Equal(expected, _filePathProviderMock.Object.GetOverwriteProtectedPath(path));
    }

    public static IEnumerable<object[]> GetOverwriteProtectedPathTestData(int number)
    {
        static string append(int i) => i < 1 ? string.Empty : $"-[{i}]";

        yield return [number, "C:\\file_that_exists.txt", $"C:\\file_that_exists{append(number)}.txt"];
        yield return [number, "C:\\config.json.sample", $"C:\\config.json{append(number)}.sample"];
        yield return [number, "C:\\config", $"C:\\config{append(number)}"];
        yield return [number, "C:\\test.txt.txt", $"C:\\test.txt{append(number)}.txt"];
        yield return [number, "C:\\nest1\\nest2\\file_that_exists.txt", $"C:\\nest1\\nest2\\file_that_exists{append(number)}.txt"];
        yield return [number, "C:\\nest1\\nest2\\config.json.sample", $"C:\\nest1\\nest2\\config.json{append(number)}.sample"];
        yield return [number, "C:\\nest1\\nest2\\config", $"C:\\nest1\\nest2\\config{append(number)}"];
        yield return [number, "C:\\nest1\\nest2\\test.txt.txt", $"C:\\nest1\\nest2\\test.txt{append(number)}.txt"];
    }

    private void SetupDoesFileExistForGetOverwriteProtectedPath(int number)
    {
        var sequence = _filePathProviderMock
            .SetupSequence(m => m.DoesFileExist(It.IsAny<string>()));

        for (int i = 0; i < number; i++)
        {
            sequence.Returns(true);
        }
        sequence.Returns(false);
    }

    [Theory, MemberData(nameof(GetFileNameWithExtensionTestData))]
    public void GetFileNameWithExtension_WhenPathIsValid_ReturnsFileNameWithExtension(string path, string expected)
    {
        Assert.Equal(expected, _filePathProviderMock.Object.GetFileNameWithExtension(path));
    }

    public static IEnumerable<object[]> GetFileNameWithExtensionTestData() =>
    [
        ["C:\\file_that_exists.txt", "file_that_exists.txt"],
            ["C:\\config.json.sample", "config.json.sample"],
            ["C:\\config", "config"],
            ["C:\\test.txt.txt", "test.txt.txt"],
            ["C:\\nest1\\nest2\\file_that_exists.txt", "file_that_exists.txt"],
            ["C:\\nest1\\nest2\\config.json.sample", "config.json.sample"],
            ["C:\\nest1\\nest2\\config", "config"],
            ["C:\\nest1\\nest2\\test.txt.txt", "test.txt.txt"]
    ];

    [Theory, MemberData(nameof(GetFileNameWithoutExtensionTestData))]
    public void GetFileNameWithoutExtension_WhenPathIsValid_ReturnsFileNameWithoutExtension(string path, string expected)
    {
        Assert.Equal(expected, _filePathProviderMock.Object.GetFileNameWithoutExtension(path));
    }

    public static IEnumerable<object[]> GetFileNameWithoutExtensionTestData() =>
    [
        ["C:\\file_that_exists.txt", "file_that_exists"],
            ["C:\\config.json.sample", "config.json"],
            ["C:\\config", "config"],
            ["C:\\test.txt.txt", "test.txt"],
            ["C:\\nest1\\nest2\\file_that_exists.txt", "file_that_exists"],
            ["C:\\nest1\\nest2\\config.json.sample", "config.json"],
            ["C:\\nest1\\nest2\\config", "config"],
            ["C:\\nest1\\nest2\\test.txt.txt", "test.txt"]
    ];
}

using Corely.Common.Providers.Files;
using Corely.UnitTests.ClassData;

namespace Corely.Common.UnitTests.Providers.Files
{
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
            string append(int i) => i < 1 ? "" : $"-[{i}]";

            yield return new object[] { number, "C:\\file_that_exists.txt", $"C:\\file_that_exists{append(number)}.txt" };
            yield return new object[] { number, "C:\\config.json.sample", $"C:\\config.json{append(number)}.sample" };
            yield return new object[] { number, "C:\\config", $"C:\\config{append(number)}" };
            yield return new object[] { number, "C:\\test.txt.txt", $"C:\\test.txt{append(number)}.txt" };
            yield return new object[] { number, "C:\\nest1\\nest2\\file_that_exists.txt", $"C:\\nest1\\nest2\\file_that_exists{append(number)}.txt" };
            yield return new object[] { number, "C:\\nest1\\nest2\\config.json.sample", $"C:\\nest1\\nest2\\config.json{append(number)}.sample" };
            yield return new object[] { number, "C:\\nest1\\nest2\\config", $"C:\\nest1\\nest2\\config{append(number)}" };
            yield return new object[] { number, "C:\\nest1\\nest2\\test.txt.txt", $"C:\\nest1\\nest2\\test.txt{append(number)}.txt" };
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

        public static IEnumerable<object[]> GetFileNameWithExtensionTestData()
        {
            yield return new object[] { "C:\\file_that_exists.txt", "file_that_exists.txt" };
            yield return new object[] { "C:\\config.json.sample", "config.json.sample" };
            yield return new object[] { "C:\\config", "config" };
            yield return new object[] { "C:\\test.txt.txt", "test.txt.txt" };
            yield return new object[] { "C:\\nest1\\nest2\\file_that_exists.txt", "file_that_exists.txt" };
            yield return new object[] { "C:\\nest1\\nest2\\config.json.sample", "config.json.sample" };
            yield return new object[] { "C:\\nest1\\nest2\\config", "config" };
            yield return new object[] { "C:\\nest1\\nest2\\test.txt.txt", "test.txt.txt" };
        }

        [Theory, MemberData(nameof(GetFileNameWithoutExtensionTestData))]
        public void GetFileNameWithoutExtension_WhenPathIsValid_ReturnsFileNameWithoutExtension(string path, string expected)
        {
            Assert.Equal(expected, _filePathProviderMock.Object.GetFileNameWithoutExtension(path));
        }

        public static IEnumerable<object[]> GetFileNameWithoutExtensionTestData()
        {
            yield return new object[] { "C:\\file_that_exists.txt", "file_that_exists" };
            yield return new object[] { "C:\\config.json.sample", "config.json" };
            yield return new object[] { "C:\\config", "config" };
            yield return new object[] { "C:\\test.txt.txt", "test.txt" };
            yield return new object[] { "C:\\nest1\\nest2\\file_that_exists.txt", "file_that_exists" };
            yield return new object[] { "C:\\nest1\\nest2\\config.json.sample", "config.json" };
            yield return new object[] { "C:\\nest1\\nest2\\config", "config" };
            yield return new object[] { "C:\\nest1\\nest2\\test.txt.txt", "test.txt" };
        }
    }
}

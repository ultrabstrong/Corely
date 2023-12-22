namespace Corely.Common.Providers.Files
{
    public class FilePathProvider : IFilePathProvider
    {
        // This method exists to allow for mocking in unit tests.
        public virtual bool DoesFileExist(string filepath)
        {
            return File.Exists(filepath);
        }

        public string GetOverwriteProtectedPath(string filepath)
        {
            if (!DoesFileExist(filepath)) { return filepath; }

            FileInfo info = new(filepath);
            FileInfo newInfo;
            int i = 0;
            do
            {
                newInfo = new($"{info.DirectoryName}\\{GetOverwriteProtectedFileName(info, ++i)}");
            }
            while (DoesFileExist(newInfo.FullName));

            return newInfo.FullName;
        }

        private string GetOverwriteProtectedFileName(FileInfo info, int appendCount)
        {
            if (string.IsNullOrWhiteSpace(info.Extension))
            {
                return $"{info.Name}-[{appendCount}]";
            }
            string fileNameWithoutExtension = RemoveLastExtensionOccurrence(info);
            return $"{fileNameWithoutExtension}-[{appendCount}]{info.Extension}";
        }

        private string RemoveLastExtensionOccurrence(FileInfo info)
        {
            int place = info.Name.LastIndexOf(info.Extension);
            if (place == -1) { return info.Name; }
            return info.Name.Remove(place, info.Extension.Length).Insert(place, "");
        }

        public string GetFileNameWithExtension(string filepath)
        {
            var info = new FileInfo(filepath);
            return info.Name;
        }

        public string GetFileNameWithoutExtension(string filepath)
        {
            var info = new FileInfo(filepath);
            return RemoveLastExtensionOccurrence(info);
        }
    }
}

namespace Corely.Shared.Core.Files
{
    public static class FilePathHelper
    {
        public static string GetOverwriteProtectedPath(string filepath)
        {
            var info = new FileInfo(filepath);
            if (!info.Exists) { return filepath; }

            FileInfo newinfo;
            int i = 0;
            do
            {
                newinfo = new($"{info.DirectoryName}\\{info.Name.Replace(info.Extension, $"-[{++i}]")}{info.Extension}");
            }
            while (newinfo.Exists);

            return newinfo.FullName;
        }

        public static string GetFileName(string filepath)
        {
            var info = new FileInfo(filepath);
            return info.Name;
        }

        public static string GetFileNameNoExt(string filepath)
        {
            var info = new FileInfo(filepath);
            int place = info.Name.LastIndexOf(info.Extension);
            if (place == -1) { return info.Name; }
            return info.Name.Remove(place, info.Extension.Length).Insert(place, "");
        }
    }
}

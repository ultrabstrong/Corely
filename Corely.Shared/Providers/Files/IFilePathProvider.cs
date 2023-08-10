namespace Corely.Shared.Providers.Files
{
    public interface IFilePathProvider
    {
        bool DoesFileExist(string filepath);
        string GetOverwriteProtectedPath(string filepath);
        string GetFileNameWithExtension(string filepath);
        string GetFileNameWithoutExtension(string filepath);
    }
}

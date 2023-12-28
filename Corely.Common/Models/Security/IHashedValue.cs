namespace Corely.Common.Models.Security
{
    public interface IHashedValue
    {
        string Hash { get; }
        void Set(string value);
        bool Verify(string value);
    }
}

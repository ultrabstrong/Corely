namespace Corely.Security.Hashing.Models;

public interface IHashedValue
{
    string Hash { get; init; }
    IHashedValue Set(string value);
    bool Verify(string value);
}

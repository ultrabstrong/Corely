namespace Corely.IAM.Security.Models;

public class SymmetricKey
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
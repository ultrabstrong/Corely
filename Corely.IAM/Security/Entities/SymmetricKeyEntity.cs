namespace Corely.IAM.Security.Entities;

public class SymmetricKeyEntity
{
    public int Id { get; set; }
    public string EncryptedKey { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
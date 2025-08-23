namespace Corely.IAM.Security.Entities;

public class AsymmetricKeyEntity
{
    public int Id { get; set; }
    public string PublicKey { get; set; } = string.Empty;
    public string EncryptedPrivateKey { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
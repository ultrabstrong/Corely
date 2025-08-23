namespace Corely.IAM.Security.Models;

public class AsymmetricKey
{
    public int Id { get; set; }
    public string PublicKey { get; set; } = string.Empty;
    public string PrivateKey { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
namespace Corely.IAM.BasicAuths.Models;

public class BasicAuth
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}
namespace Corely.IAM.Users.Results;

public class CreateUserResult
{
    public int UserId { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
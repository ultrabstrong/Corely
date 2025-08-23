namespace Corely.IAM.Users.Requests;

public class CreateUserRequest
{
    public string Username { get; }
    public string Email { get; }

    public CreateUserRequest(string username, string email)
    {
        Username = username;
        Email = email;
    }
}
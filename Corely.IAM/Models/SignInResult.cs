namespace Corely.IAM.Models;

public record SignInResult(
    bool IsSuccess,
    string? Message,
    string? AuthToken);

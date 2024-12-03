namespace Corely.IAM.Models;

public abstract record ResultBase(
    bool IsSuccess,
    string? Message);

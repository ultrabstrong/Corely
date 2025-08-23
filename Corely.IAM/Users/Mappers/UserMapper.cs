using Corely.IAM.Users.Models;
using Corely.IAM.Users.Requests;

namespace Corely.IAM.Users.Mappers;

internal static class UserMapper
{
    public static User ToModel(CreateUserRequest source)
    {
        if (source == null) return null!;
        
        return new User
        {
            Username = source.Username,
            Email = source.Email,
            CreatedAt = DateTime.UtcNow
        };
    }
}
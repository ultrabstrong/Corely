using Corely.IAM.BasicAuths.Entities;
using Corely.IAM.BasicAuths.Models;

namespace Corely.IAM.BasicAuths.Mappers;

internal static class BasicAuthMapper
{
    public static BasicAuthEntity ToEntity(BasicAuth source)
    {
        if (source == null) return null!;
        
        return new BasicAuthEntity
        {
            Id = source.Id,
            Username = source.Username,
            Password = source.Password,
            CreatedAt = source.CreatedAt,
            LastLoginAt = source.LastLoginAt
        };
    }

    public static BasicAuth ToModel(BasicAuthEntity source)
    {
        if (source == null) return null!;
        
        return new BasicAuth
        {
            Id = source.Id,
            Username = source.Username,
            Password = source.Password,
            CreatedAt = source.CreatedAt,
            LastLoginAt = source.LastLoginAt
        };
    }
}
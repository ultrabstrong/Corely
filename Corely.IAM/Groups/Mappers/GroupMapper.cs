using Corely.IAM.Groups.Entities;
using Corely.IAM.Groups.Models;

namespace Corely.IAM.Groups.Mappers;

internal static class GroupMapper
{
    public static GroupEntity ToEntity(Group source)
    {
        if (source == null) return null!;
        
        return new GroupEntity
        {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description
        };
    }

    public static Group ToModel(GroupEntity source)
    {
        if (source == null) return null!;
        
        return new Group
        {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description
        };
    }
}
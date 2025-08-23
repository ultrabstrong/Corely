# AutoMapper to Custom Mappers Migration - Usage Guide

This document shows how the migration from AutoMapper to custom static mappers works.

## Before (AutoMapper)

```csharp
// AutoMapper Profile
internal class GroupProfile : Profile
{
    public GroupProfile()
    {
        CreateMap<Group, GroupEntity>(MemberList.Source)
            .ReverseMap();
    }
}

// Usage
var mapper = serviceProvider.GetService<IMapper>();
var entity = mapper.Map<GroupEntity>(group);
var model = mapper.Map<Group>(entity);
```

## After (Custom Mappers)

```csharp
// Static Mapper Class
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

// Usage (same interface!)
var mapProvider = serviceProvider.GetService<IMapProvider>();
var entity = mapProvider.MapTo<GroupEntity>(group);
var model = mapProvider.MapTo<Group>(entity);
```

## Key Benefits

1. **No AutoMapper license required** - Eliminates licensing costs
2. **Better performance** - No reflection overhead during mapping
3. **Compile-time safety** - Type errors caught at compile time
4. **Minimal code changes** - Same IMapProvider interface
5. **Transparent encryption** - Handles encryption/decryption automatically

## Encryption Mapping Example

```csharp
// Handles encryption automatically
var symmetricKey = new SymmetricKey { Key = "plaintext-key" };
var entity = mapProvider.MapTo<SymmetricKeyEntity>(symmetricKey);
// entity.EncryptedKey is now encrypted

var decryptedKey = mapProvider.MapTo<SymmetricKey>(entity);
// decryptedKey.Key is decrypted back to "plaintext-key"
```
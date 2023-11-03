using Corely.Shared.Extensions;
using System.Reflection;

using CorelyForeignKeyAttribute = Corely.Shared.Attributes.Db.ForeignKeyAttribute;

namespace Corely.Shared.Mappers.Liquibase.EntityMappers.Providers
{
    internal class EntityForeignKeyProvider : IEntityForeignKeyProvider
    {
        private readonly HashSet<Type> _entities;
        private readonly Dictionary<Type, Dictionary<string, CorelyForeignKeyAttribute>> _foreignKeys;
        private readonly List<IPropertyInfoForeignKeyProvider> _providers = new()
        {
            new PropertyInfoFkProviderForCorelyAttr(),
            new PropertyInfoFkProviderForSystemAttr(),
            //new PropertyInfoFkProviderForNavProperty(),
        };

        public EntityForeignKeyProvider(IEnumerable<Type> entites)
        {
            _entities = entites
                .ThrowIfNull(nameof(entites))
                .ToHashSet();

            _foreignKeys = new Dictionary<Type, Dictionary<string, CorelyForeignKeyAttribute>>();

            FindForeignKeys();
        }

        private void FindForeignKeys()
        {
            foreach (var entity in _entities)
            {
                foreach (var prop in entity.GetProperties())
                {
                    var (ownerEntity, corelyFkAttr) = GetCorelyForeignKeyAttr(prop);
                    if (corelyFkAttr != null)
                    {
                        if (!_foreignKeys.TryGetValue(ownerEntity, out var ownerEntityFks))
                        {
                            ownerEntityFks = new Dictionary<string, CorelyForeignKeyAttribute>();
                            _foreignKeys.Add(ownerEntity, ownerEntityFks);
                        }

                        if (!ownerEntityFks.ContainsKey(prop.Name))
                        {
                            ownerEntityFks.Add(prop.Name, corelyFkAttr);
                        }
                    }
                }
            }
        }

        private (Type, CorelyForeignKeyAttribute?) GetCorelyForeignKeyAttr(PropertyInfo prop)
        {
            foreach (var provider in _providers)
            {
                var (ownerEntity, corelyFkAttr) = provider.GetCorelyForeignKeyAttr(prop);
                if (corelyFkAttr != null)
                {
                    return (ownerEntity, corelyFkAttr);
                }
            }

            return (
                prop.DeclaringType
                    ?? throw new Exception($"Property {prop.Name} does nto belong to an entity"),
                null);
        }

        public CorelyForeignKeyAttribute? Get(Type entity, string propertyName)
        {
            if (_foreignKeys.TryGetValue(entity, out var foreignKeys))
            {
                if (foreignKeys.TryGetValue(propertyName, out var foreignKey))
                {
                    return foreignKey;
                }
            }
            return null;
        }
    }
}

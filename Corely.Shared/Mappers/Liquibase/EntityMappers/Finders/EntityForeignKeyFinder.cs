using Corely.Shared.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

using CorelyForeignKeyAttribute = Corely.Shared.Attributes.Db.ForeignKeyAttribute;
using SystemForeignKeyAttribute = System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute;

namespace Corely.Shared.Mappers.Liquibase.EntityMappers.Finders
{
    internal class EntityForeignKeyFinder : IEntityForeignKeyFinder
    {
        private readonly HashSet<Type> _entities;
        private readonly Dictionary<Type, Dictionary<string, CorelyForeignKeyAttribute>> _foreignKeys;

        public EntityForeignKeyFinder(ICollection<Type> entites)
        {
            _entities = entites
                .ThrowIfNull(nameof(entites))
                .ToHashSet();

            _foreignKeys = new Dictionary<Type, Dictionary<string, CorelyForeignKeyAttribute>>();

            FindForeignKeys();
        }

        public CorelyForeignKeyAttribute? Find(Type entity, string propertyName)
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

        private void FindForeignKeys()
        {
            foreach (var entity in _entities)
            {
                foreach (var prop in entity.GetProperties())
                {
                    if (GetForeignKeyAttr(prop, out var corelyFkAttr)
                        && corelyFkAttr != null)
                    {
                        if (!_foreignKeys.TryGetValue(entity, out var entityFks))
                        {
                            entityFks = new Dictionary<string, CorelyForeignKeyAttribute>();
                            _foreignKeys.Add(entity, entityFks);
                        }

                        if (!entityFks.ContainsKey(prop.Name))
                        {
                            entityFks.Add(prop.Name, corelyFkAttr);
                        }
                    }
                }
            }
        }

        private bool GetForeignKeyAttr(PropertyInfo prop, out CorelyForeignKeyAttribute? corelyFkAttr)
        {
            corelyFkAttr = prop.GetCustomAttribute<CorelyForeignKeyAttribute>();

            if (corelyFkAttr == null)
            {
                var systemFkAttr = prop.GetCustomAttribute<SystemForeignKeyAttribute>();
                if (systemFkAttr != null)
                {
                    var entity = prop.PropertyType;
                    var table = entity.GetCustomAttribute<TableAttribute>()?.Name ?? entity.Name;

                    if (!string.IsNullOrWhiteSpace(table))
                    {
                        corelyFkAttr = new CorelyForeignKeyAttribute(table, systemFkAttr.Name);
                    }
                }
            }

            // Todo: finish implementing for navigation properties

            return corelyFkAttr != null;
        }
    }
}

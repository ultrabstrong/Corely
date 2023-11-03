using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using CorelyForeignKeyAttribute = Corely.Shared.Attributes.Db.ForeignKeyAttribute;
using SystemForeignKeyAttribute = System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute;

namespace Corely.Shared.Mappers.Liquibase.EntityMappers.Providers
{
    internal class PropertyInfoFkProviderForSystemAttr : IPropertyInfoForeignKeyProvider
    {
        public (Type, CorelyForeignKeyAttribute?) GetCorelyForeignKeyAttr(PropertyInfo prop)
        {
            Type foreignKeyOwnerEntity = prop.DeclaringType ?? throw new Exception($"Property {prop.Name} does nto belong to an entity");
            CorelyForeignKeyAttribute? corelyFkAttr = null;

            var systemFkAttr = prop.GetCustomAttribute<SystemForeignKeyAttribute>();
            if (systemFkAttr != null)
            {
                Type declaringEntity = foreignKeyOwnerEntity;
                Type navigationEntity;
                string principleTableName;
                string[] dependantColumns;

                if (prop.PropertyType.IsPrimitive)
                {
                    // case 1 : attribute on `dependent.key` and references `dependent.navigation`
                    var navigationProp = declaringEntity.GetProperty(systemFkAttr.Name)
                        ?? throw new Exception($"Could not find navigation property `{declaringEntity.Name}.{systemFkAttr.Name}` for key property `{declaringEntity.Name}.{prop.Name}`");

                    navigationEntity = GetNavigationEntity(navigationProp);
                    principleTableName = navigationEntity.GetCustomAttribute<TableAttribute>()?.Name ?? navigationEntity.Name;
                    dependantColumns = new[] { GetColumnName(prop) };
                    VerifyPrincipleEntityReferencePropertiesExist(new[] { prop.Name }, navigationEntity);
                    foreignKeyOwnerEntity = declaringEntity;
                }
                else
                {
                    // case 2 : attribute on `???.navigation` and references `dependent.key`
                    navigationEntity = GetNavigationEntity(prop);
                    if (IsInverseNavigation(prop, navigationEntity, systemFkAttr.Name))
                    {
                        // case 2.1 : attribute on `principle.navigation` and references `dependent.key` (inverse navigation)
                        principleTableName = declaringEntity.GetCustomAttribute<TableAttribute>()?.Name ?? declaringEntity.Name;
                        dependantColumns = GetFkColumnNames(navigationEntity, systemFkAttr, prop);
                        VerifyPrincipleEntityReferencePropertiesExist(systemFkAttr.Name.Split(','), declaringEntity);
                        foreignKeyOwnerEntity = navigationEntity;

                    }
                    else
                    {
                        // case 2.2 : attribute on `dependent.navigation` and references `dependent.key`
                        principleTableName = navigationEntity.GetCustomAttribute<TableAttribute>()?.Name ?? navigationEntity.Name;
                        dependantColumns = GetFkColumnNames(declaringEntity, systemFkAttr, prop);
                        VerifyPrincipleEntityReferencePropertiesExist(systemFkAttr.Name.Split(','), navigationEntity);
                        foreignKeyOwnerEntity = declaringEntity;
                    }
                }

                corelyFkAttr = new(principleTableName, dependantColumns);
            }
            return (foreignKeyOwnerEntity, corelyFkAttr);
        }

        private bool IsInverseNavigation(
            PropertyInfo navigationProp,
            Type navigationEntityType,
            string keyPropName)
        {
            bool isInverse = false;

            Type? declaringEntityType = navigationProp.DeclaringType;
            if (declaringEntityType?.GetProperty(keyPropName) == null)
            {
                isInverse = true;
                if (navigationEntityType?.GetProperty(keyPropName) == null)
                {
                    throw new Exception($"Key property not found at `{declaringEntityType?.Name}.{keyPropName}` or `{navigationEntityType?.Name}.{keyPropName}` for navigation property `{declaringEntityType?.Name}{navigationProp.Name}`");
                }
            }

            return isInverse;
        }

        private Type GetNavigationEntity(PropertyInfo dependentNavigationProp)
        {
            var principleEntityType = dependentNavigationProp.PropertyType;

            bool isEnumerable = typeof(System.Collections.IEnumerable)
                .IsAssignableFrom(dependentNavigationProp.PropertyType);

            if (isEnumerable)
            {
                Type? genericIEnum = dependentNavigationProp.PropertyType
                    .GetInterfaces()
                    .FirstOrDefault(t => t.IsGenericType
                        && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

                Type? enumerableType = genericIEnum?.GetGenericArguments()[0];

                if (enumerableType != null)
                {
                    principleEntityType = enumerableType;
                }
            }

            return principleEntityType;
        }

        private string[] GetFkColumnNames(
            Type? dependentEntityType,
            SystemForeignKeyAttribute systemFkAttr,
            PropertyInfo navigationProp)
        {
            List<string> foreignKeyColumnNames = new();

            string[] keyPropertyNames = systemFkAttr.Name.Split(',');
            foreach (string keyPropertyName in keyPropertyNames)
            {
                var keyProperty = dependentEntityType?.GetProperty(systemFkAttr.Name)
                    ?? throw new Exception($"Could not find key property `{dependentEntityType?.Name}.{systemFkAttr.Name}` for navigation property `{navigationProp.DeclaringType?.Name}.{navigationProp.Name}`");

                foreignKeyColumnNames.Add(GetColumnName(keyProperty));
            }

            return foreignKeyColumnNames.ToArray();
        }

        private string GetColumnName(PropertyInfo prop)
        {
            var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
            return !string.IsNullOrWhiteSpace(columnAttr?.Name)
                ? columnAttr.Name
                : prop.Name;
        }

        private void VerifyPrincipleEntityReferencePropertiesExist(string[] fkPropertyNames, Type principleEntityType)
        {
            var principleProperties = principleEntityType.GetProperties();
            foreach (string fkName in fkPropertyNames)
            {
                List<string> columnsChecked = new();

                if (principleProperties.Any(p => p.Name == fkName))
                {
                    continue;
                }
                columnsChecked.Add(fkName);

                if (fkName.Length > principleEntityType.Name.Length &&
                    fkName.StartsWith(principleEntityType.Name))
                {
                    var columnNameShort = fkName.Remove(0, principleEntityType.Name.Length);
                    if (principleEntityType.GetProperty(columnNameShort) != null)
                    {
                        continue;
                    }
                    columnsChecked.Add(columnNameShort);
                }

                if (fkPropertyNames.Length == 1)
                {
                    if (principleProperties.Any(p => p.Name == "Id"))
                    {
                        continue;
                    }
                    columnsChecked.Add("Id");

                    if (principleProperties.Any(p => p.Name == $"{principleEntityType.Name}Id"))
                    {
                        continue;
                    }
                    columnsChecked.Add($"{principleEntityType.Name}Id");

                    if (principleProperties.Any(p => Attribute.IsDefined(p, typeof(KeyAttribute))))
                    {
                        continue;
                    }
                    columnsChecked.Add("[Key] attribute");
                }

                throw new Exception($"Could not find column `{principleEntityType.Name}.[{string.Join("|", columnsChecked)}]` referenced by foreign key `{fkName}`");
            }
        }
    }
}

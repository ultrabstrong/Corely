using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using CorelyForeignKeyAttribute = Corely.Shared.Attributes.Db.ForeignKeyAttribute;
using SystemForeignKeyAttribute = System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute;

namespace Corely.Shared.Mappers.Liquibase.EntityMappers.Providers
{
    internal class PropertyInfoSystemFkProvider : IPropertyInfoForeignKeyProvider
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
                    // Todo: Verify the dependant column is also in principle table
                    foreignKeyOwnerEntity = declaringEntity;
                }
                else
                {
                    navigationEntity = GetNavigationEntity(prop);
                    if (IsInverseNavigation(prop, navigationEntity, systemFkAttr.Name))
                    {
                        // case 3 : attribute on `principle.navigation` and references `dependent.key` (inverse navigation)
                        principleTableName = declaringEntity.GetCustomAttribute<TableAttribute>()?.Name ?? declaringEntity.Name;
                        dependantColumns = GetColumnNames(navigationEntity, systemFkAttr, prop);
                        // Todo: Verify the dependant column is also in principle table
                        foreignKeyOwnerEntity = navigationEntity;

                    }
                    else
                    {
                        // case 2 : attribute on `dependent.navigation` and references `dependent.key`
                        principleTableName = navigationEntity.GetCustomAttribute<TableAttribute>()?.Name ?? navigationEntity.Name;
                        dependantColumns = GetColumnNames(declaringEntity, systemFkAttr, prop);
                        // Todo: Verify the dependant column is also in principle table
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

        private string[] GetColumnNames(
            Type? dependentEntityType,
            SystemForeignKeyAttribute systemFkAttr,
            PropertyInfo navigationProp)
        {
            List<string> keyNames = new();

            string[] keyPropertyNames = systemFkAttr.Name.Split(',');
            foreach (string keyPropertyName in keyPropertyNames)
            {
                var keyProperty = dependentEntityType?.GetProperty(systemFkAttr.Name)
                    ?? throw new Exception($"Could not find key property `{dependentEntityType?.Name}.{systemFkAttr.Name}` for navigation property `{navigationProp.DeclaringType?.Name}.{navigationProp.Name}`");

                keyNames.Add(GetColumnName(keyProperty));
            }

            return keyNames.ToArray();
        }

        private string GetColumnName(PropertyInfo prop)
        {
            var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
            return !string.IsNullOrWhiteSpace(columnAttr?.Name)
                ? columnAttr.Name
                : prop.Name;
        }

        private void VerifyPrinipleEntityReferencedColumnsExist(string[] columnNames, Type principleEntityType)
        {
            foreach (string columnName in columnNames)
            {
                if (principleEntityType.GetProperty(columnName) == null)
                {
                    throw new Exception($"Could not find column `{principleEntityType.Name}.{columnName}` referenced by foreign key");
                }
            }
        }

    }
}

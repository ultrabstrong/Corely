using System.Reflection;
using CorelyForeignKeyAttribute = Corely.Shared.Attributes.Db.ForeignKeyAttribute;

namespace Corely.Shared.Mappers.Liquibase.EntityMappers.Providers
{
    internal class PropertyInfoFkProviderForCorelyAttr : IPropertyInfoForeignKeyProvider
    {
        public (Type, CorelyForeignKeyAttribute?) GetCorelyForeignKeyAttr(PropertyInfo prop)
        {
            var declaringType = prop.DeclaringType
                ?? throw new Exception("Declaring type is null");

            return (declaringType, prop.GetCustomAttribute<CorelyForeignKeyAttribute>());
        }
    }
}

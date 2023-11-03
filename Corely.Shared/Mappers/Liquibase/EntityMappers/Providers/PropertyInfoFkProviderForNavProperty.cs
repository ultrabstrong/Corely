using System.Reflection;
using CorelyForeignKeyAttribute = Corely.Shared.Attributes.Db.ForeignKeyAttribute;

namespace Corely.Shared.Mappers.Liquibase.EntityMappers.Providers
{
    internal class PropertyInfoFkProviderForNavProperty : IPropertyInfoForeignKeyProvider
    {
        public (Type, CorelyForeignKeyAttribute?) GetCorelyForeignKeyAttr(PropertyInfo prop)
        {
            throw new NotImplementedException();
        }
    }
}

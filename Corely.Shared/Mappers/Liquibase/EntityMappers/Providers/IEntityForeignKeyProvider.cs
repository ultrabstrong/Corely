using CorelyForeignKeyAttribute = Corely.Shared.Attributes.Db.ForeignKeyAttribute;

namespace Corely.Shared.Mappers.Liquibase.EntityMappers.Providers
{
    internal interface IEntityForeignKeyProvider
    {
        public CorelyForeignKeyAttribute? Get(Type entity, string propertyName);
    }
}

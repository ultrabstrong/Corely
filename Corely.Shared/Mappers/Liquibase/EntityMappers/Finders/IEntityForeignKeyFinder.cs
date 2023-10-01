using Corely.Shared.Attributes.Db;

namespace Corely.Shared.Mappers.Liquibase.EntityMappers.Finders
{
    internal interface IEntityForeignKeyFinder
    {
        public ForeignKeyAttribute? Find(Type entity, string propertyName);
    }
}

namespace Corely.Shared.Mappers.Liquibase
{
    internal interface IEntityToLiquibaseMapper
    {
        public string MapEntitiesInNamespace(string rootEntityNamespace);
    }
}

namespace Corely.Shared.Mappers.Liquibase
{
    public interface IEntityToLiquibaseMapper
    {
        public string MapEntitiesInNamespace(string rootEntityNamespace);
    }
}

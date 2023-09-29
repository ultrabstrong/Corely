using System.Text.Json;
using System.Text.Json.Serialization;

namespace Corely.Shared.Mappers.Liquibase.EntityMappers
{
    public sealed class EntityToLiquibaseJsonMapper : EntityToLiquibaseMapperBase
    {
        public EntityToLiquibaseJsonMapper(string assemblyPath, string rootEntityNamespace)
            : base(assemblyPath, rootEntityNamespace)
        {
        }

        public override string Map()
        {
            var changeLog = MapDatabaseChangeLog();

            var changeLogJson = JsonSerializer.Serialize(
                changeLog,
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

            return changeLogJson;
        }
    }
}

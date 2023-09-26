﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Corely.Shared.Mappers.Liquibase
{
    public class EntityToLiquibaseJsonMapper : EntityToLiquibaseMapperBase
    {
        public EntityToLiquibaseJsonMapper(string assemblyPath)
            : base(assemblyPath)
        {
        }

        public override string MapEntitiesInNamespace(string rootEntityNamespace)
        {
            var entities = FindEntitiesInNamespace(rootEntityNamespace);
            var createTables = MapCreateTables(entities);

            var createTableJson = JsonSerializer.Serialize(
                createTables,
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

            return createTableJson;
        }
    }
}

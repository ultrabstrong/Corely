using Corely.Shared.Mappers.Liquibase.Models;
using System.Reflection;

namespace Corely.Shared.Mappers.Liquibase.EntityMappers
{
    public abstract class EntityToLiquibaseMapperBase : ILiquibaseMapper
    {
        private readonly string _assemblyPath;
        private readonly string _rootEntityNamespace;
        private string _id = Guid.NewGuid().ToString();
        private string _author = "Corely";

        public string Id
        {
            get => _id;
            init
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _id = value;
                }
            }
        }
        public string Author
        {
            get => _author; init
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _author = value;
                }
            }
        }

        internal EntityToLiquibaseMapperBase(string assemblyPath, string rootEntityNamespace)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(assemblyPath, nameof(assemblyPath));
            ArgumentException.ThrowIfNullOrWhiteSpace(rootEntityNamespace, nameof(rootEntityNamespace));

            _assemblyPath = assemblyPath;
            _rootEntityNamespace = rootEntityNamespace;
        }

        public abstract string Map();

        protected LiquibaseDatabaseChangeLog MapDatabaseChangeLog()
        {
            return new LiquibaseDatabaseChangeLog
            {
                ChangeSets = new List<LiquibaseChangeSet>
                {
                    MapChangeset()
                }
            };
        }

        private LiquibaseChangeSet MapChangeset()
        {
            return new LiquibaseChangeSet
            {
                Id = Id,
                Author = Author,
                Changes = MapChanges(FindEntitiesInNamespace())
            };
        }

        private List<Type> FindEntitiesInNamespace()
        {
            var assembly = Assembly.LoadFrom(_assemblyPath);

            var entities = assembly
                .GetTypes()
                .Where(type => type.Namespace?.StartsWith(_rootEntityNamespace, StringComparison.Ordinal) == true)
                .ToList();

            return entities;
        }

        private IEnumerable<LiquibaseChange> MapChanges(List<Type> entities)
        {
            foreach (var entity in entities)
            {
                var tableMapper = new EntityToLiquibaseTableMapper(entity);
                yield return new LiquibaseChange
                {
                    CreateTable = tableMapper.Map()
                };
            }
        }
    }
}

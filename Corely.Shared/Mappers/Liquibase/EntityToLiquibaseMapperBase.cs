using Corely.Shared.Models.Liquibase;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Corely.Shared.Mappers.Liquibase
{
    public abstract class EntityToLiquibaseMapperBase : IEntityToLiquibaseMapper
    {
        public abstract string MapEntitiesInNamespace(string rootEntityNamespace);

        protected internal virtual List<Type> FindEntitiesInNamespace(string rootEntityNamespace)
        {
            var entities = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => string.Equals(type.Namespace, rootEntityNamespace, StringComparison.Ordinal))
                .ToList();
            return entities;
        }

        protected internal virtual IEnumerable<LiquibaseCreateTable> MapToClasses(List<Type> entities)
        {
            foreach (var entity in entities)
            {
                yield return MapToClass(entity);
            }
        }

        protected internal virtual LiquibaseCreateTable MapToClass(Type entity)
        {
            var createTable = new LiquibaseCreateTable
            {
                TableName = entity.GetCustomAttribute<TableAttribute>()?.Name ?? entity.Name,
                Columns = new List<LiquibaseColumn>()
            };

            foreach (var prop in entity.GetProperties())
            {
                var column = new LiquibaseColumn
                {
                    Name = prop.Name,
                    Type = MapDotNetTypeToSqlType(prop.PropertyType),
                    Constraints = new LiquibaseConstraints
                    {
                        PrimaryKey = prop.GetCustomAttributes<KeyAttribute>().Any(),
                        Nullable = !prop.GetCustomAttributes<RequiredAttribute>().Any()
                    }
                };

                createTable.Columns.Add(column);
            }

            return createTable;
        }

        protected internal virtual string MapDotNetTypeToSqlType(Type type)
        {
            Type underlyingType = Nullable.GetUnderlyingType(type) ?? type;

            return underlyingType switch
            {
                Type t when t == typeof(int) => "INT",
                Type t when t == typeof(string) => "VARCHAR",
                Type t when t == typeof(bool) => "BOOLEAN",
                Type t when t == typeof(DateTime) => "DATETIME",
                Type t when t == typeof(float) => "FLOAT",
                Type t when t == typeof(double) => "DOUBLE",
                Type t when t == typeof(decimal) => "DECIMAL",
                _ => "UNKNOWN_TYPE",
            };
        }
    }
}

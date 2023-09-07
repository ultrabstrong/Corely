using Corely.Shared.Attributes.Db;
using Corely.Shared.Mappers.Liquibase.Models;
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

        protected internal virtual IEnumerable<LiquibaseCreateTable> MapCreateTables(List<Type> entities)
        {
            foreach (var entity in entities)
            {
                yield return MapCreateTable(entity);
            }
        }

        protected internal virtual LiquibaseCreateTable MapCreateTable(Type entity)
        {
            var createTable = new LiquibaseCreateTable
            {
                TableName = entity.GetCustomAttribute<TableAttribute>()?.Name ?? entity.Name,
                Columns = new List<LiquibaseColumn>()
            };

            createTable.Columns.AddRange(MapColumns(entity));

            return createTable;
        }

        protected internal virtual IEnumerable<LiquibaseColumn> MapColumns(Type entity)
        {
            foreach (var prop in entity.GetProperties())
            {
                var column = MapColumn(prop);
                if (column != null)
                {
                    yield return column;
                }
            }
        }

        protected internal virtual LiquibaseColumn? MapColumn(PropertyInfo prop)
        {
            if (MapSqlType(prop.PropertyType, out string sqlType))
            {
                var column = new LiquibaseColumn
                {
                    Name = prop.Name,
                    Type = sqlType,
                    Constraints = MapConstraints(prop)
                };

                return column;
            }

            return null;
        }

        protected internal virtual bool MapSqlType(Type type, out string sqlType)
        {
            Type underlyingType = Nullable.GetUnderlyingType(type) ?? type;

            sqlType = underlyingType switch
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

            return sqlType != "UNKNOWN_TYPE";
        }

        protected internal virtual LiquibaseConstraints MapConstraints(PropertyInfo prop)
        {
            var constraints = new LiquibaseConstraints
            {
                PrimaryKey = prop.GetCustomAttributes<KeyAttribute>().Any(),
                Nullable = !prop.GetCustomAttributes<RequiredAttribute>().Any(),
                Unique = prop.GetCustomAttributes<UniqueAttribute>().Any(),
                ForeignKey = prop.GetCustomAttributes<ForeignKeyAttribute>().FirstOrDefault()?.Name,
                //ForeignKeyTable = prop.GetCustomAttributes<ForeignKeyAttribute>().FirstOrDefault()?.NavigationProperty,
                CheckExpression = prop.GetCustomAttributes<CheckAttribute>().FirstOrDefault()?.Expression,
                Default = prop.GetCustomAttributes<DefaultAttribute>().FirstOrDefault()?.Value,
                AutoIncrement = prop.GetCustomAttributes<AutoIncrementAttribute>().Any()
            };

            return constraints;
        }
    }
}

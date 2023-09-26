using Corely.Shared.Attributes.Db;
using Corely.Shared.Mappers.Liquibase.Constraints;
using Corely.Shared.Mappers.Liquibase.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Corely.Shared.Mappers.Liquibase
{
    public abstract class EntityToLiquibaseMapperBase : IEntityToLiquibaseMapper
    {
        private readonly string _assemblyPath;

        protected EntityToLiquibaseMapperBase(string assemblyPath)
        {
            _assemblyPath = assemblyPath;
        }

        public abstract string MapEntitiesInNamespace(string rootEntityNamespace);

        protected internal virtual List<Type> FindEntitiesInNamespace(string rootEntityNamespace)
        {
            var assembly = Assembly.LoadFrom(_assemblyPath);

            var entities = assembly
                .GetTypes()
                .Where(type => type.Namespace?.StartsWith(rootEntityNamespace, StringComparison.Ordinal) == true)
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

        private LiquibaseCreateTable MapCreateTable(Type entity)
        {
            var tableName = entity.GetCustomAttribute<TableAttribute>()?.Name ?? entity.Name;

            var createTable = new LiquibaseCreateTable
            {
                TableName = tableName,
                Columns = MapColumns(entity, tableName)
            };

            return createTable;
        }

        private List<LiquibaseColumn> MapColumns(Type entity, string tableName)
        {
            List<LiquibaseColumn> columns = new();
            List<LiquibaseForeignKey> foreignKeys = new();

            foreach (var prop in entity.GetProperties())
            {
                if (MapColumn(prop, tableName, out var column))
                {
                    columns.Add(column);
                }
                else if (MapNavigationPropertyForeignKey(prop, out LiquibaseForeignKey foreignKey))
                {
                    foreignKeys.Add(foreignKey);
                }
            }

            return columns;
        }

        private bool MapColumn(PropertyInfo prop, string tableName, out LiquibaseColumn column)
        {
            column = null;
            var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
            var autoIncrementAttr = GetAutoIncrementAttribute(prop);

            var sqlType = columnAttr?.TypeName ?? MapSqlType(prop);
            var columnName = columnAttr?.Name ?? prop.Name;

            if (sqlType != "UNKNOWN_TYPE")
            {
                column = new LiquibaseColumn
                {
                    Name = columnName,
                    Type = sqlType,
                    Value = null,
                    AfterColumn = null,
                    AutoIncrement = autoIncrementAttr != null,
                    BeforeColumn = null,
                    Computed = null,
                    DefaultValue = null,
                    DefaultValueBoolean = null,
                    DefaultValueComputed = null,
                    DefaultValueConstraintName = null,
                    DefaultValueDate = null,
                    DefaultValueNumeric = null,
                    Descending = null,
                    Encoding = null,
                    IncrementBy = autoIncrementAttr?.IncrementBy,
                    Position = columnAttr?.Order,
                    Remarks = null,
                    StartWith = autoIncrementAttr?.StartWith,
                    ValueBlobFile = null,
                    ValueBoolean = null,
                    ValueClobFile = null,
                    ValueComputed = null,
                    ValueDate = null,
                    ValueNumeric = null,
                    Constraints = MapConstraints(prop, tableName, columnName)
                };
            }

            return column != null;
        }

        private string MapSqlType(PropertyInfo prop)
        {
            Type underlyingType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
            var maxLength = prop.GetCustomAttribute<MaxLengthAttribute>()?.Length;

            return underlyingType switch
            {
                Type t when t == typeof(int) => "INT",
                Type t when t == typeof(string) => (maxLength.HasValue ? "VARCHAR" : $"VARCHAR({maxLength})"),
                Type t when t == typeof(bool) => "BOOLEAN",
                Type t when t == typeof(DateTime) => "DATETIME",
                Type t when t == typeof(float) => "FLOAT",
                Type t when t == typeof(double) => "DOUBLE",
                Type t when t == typeof(decimal) => "DECIMAL",
                _ => "UNKNOWN_TYPE",
            };
        }

        private AutoIncrementAttribute? GetAutoIncrementAttribute(PropertyInfo prop)
        {
            var autoIncrementAttribute = prop.GetCustomAttribute<AutoIncrementAttribute>();
            if (autoIncrementAttribute == null)
            {
                if (prop.GetCustomAttribute<DatabaseGeneratedAttribute>() != null)
                {
                    autoIncrementAttribute = new AutoIncrementAttribute();
                }
            }

            return autoIncrementAttribute;
        }

        private LiquibaseConstraints MapConstraints(PropertyInfo prop, string tableName, string columnName)
        {
            var constraints = new LiquibaseConstraints();

            // Todo : implement

            return constraints;
        }

        private bool MapNavigationPropertyForeignKey(PropertyInfo prop, out LiquibaseForeignKey foreignKey)
        {
            foreignKey = null;

            var foreignKeyAttr = prop.GetCustomAttributes<ForeignKeyAttribute>().FirstOrDefault();
            if (foreignKeyAttr != null)
            {
                var entity = prop.PropertyType;
                var table = entity.GetCustomAttribute<TableAttribute>()?.Name ?? entity.Name;

                if (!string.IsNullOrWhiteSpace(table))
                {
                    foreignKey = new LiquibaseForeignKey
                    {
                        Table = table,
                        Column = foreignKeyAttr.Name
                    };
                }
            }
            return foreignKey != null;
        }
    }
}

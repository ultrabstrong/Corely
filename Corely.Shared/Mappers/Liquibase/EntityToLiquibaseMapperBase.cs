using Corely.Shared.Attributes.Db;
using Corely.Shared.Mappers.Liquibase.Constraints;
using Corely.Shared.Mappers.Liquibase.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Corely.Shared.Mappers.Liquibase
{
    public abstract class EntityToLiquibaseMapperBase : IEntityToLiquibaseMapper
    {
        private const string
            _checkConstraintPrefix = "CK_",
            _defaultConstraintPrefix = "DF_",
            _foreignKeyConstraintPrefix = "FK_",
            _indexConstraintPrefix = "IX_",
            _notNullConstraintPrefix = "NN_",
            _primaryKeyConstraintPrefix = "PK_",
            _uniqueConstraintPrefix = "UQ_";

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
            var columnName = columnAttr?.Name ?? prop.Name;
            var sqlType = columnAttr?.TypeName ?? MapSqlType(prop);

            var databaseGeneratedAttr = prop.GetCustomAttribute<DatabaseGeneratedAttribute>();
            var autoIncrementAttr = GetAutoIncrementAttribute(prop, databaseGeneratedAttr);
            var columnPlacementAttr = prop.GetCustomAttribute<ColumnPlacementAttribute>();
            var descendingIndexAttr = prop.GetCustomAttribute<DescendingIndexAttribute>();
            var remarksAttr = prop.GetCustomAttribute<RemarksAttribute>();


            if (sqlType != "UNKNOWN_TYPE")
            {
                column = new LiquibaseColumn
                {
                    Name = columnName,
                    Type = sqlType,
                    Value = null,           // Primarily used for updating records in existing databases when applying changesets that add columns. Need to add changeset functionality before implementing this. 
                    AfterColumn = columnPlacementAttr?.AfterColumn,
                    AutoIncrement = autoIncrementAttr != null,
                    BeforeColumn = columnPlacementAttr?.BeforeColumn,
                    Computed = databaseGeneratedAttr?.DatabaseGeneratedOption == DatabaseGeneratedOption.Computed,
                    Descending = descendingIndexAttr != null,
                    Encoding = null,        // Primarily used for updating records in existing databases when applying changesets that add columns. Need to add changeset functionality before implementing this. 
                    IncrementBy = autoIncrementAttr?.IncrementBy,
                    Position = columnAttr?.Order,
                    Remarks = remarksAttr?.Remarks,
                    StartWith = autoIncrementAttr?.StartWith,
                    ValueBlobFile = null,   // Primarily used for updating records in existing databases when applying changesets that add columns. Need to add changeset functionality before implementing this. 
                    ValueBoolean = null,    // Primarily used for updating records in existing databases when applying changesets that add columns. Need to add changeset functionality before implementing this. 
                    ValueClobFile = null,   // Primarily used for updating records in existing databases when applying changesets that add columns. Need to add changeset functionality before implementing this. 
                    ValueComputed = null,   // Primarily used for updating records in existing databases when applying changesets that add columns. Need to add changeset functionality before implementing this. 
                    ValueDate = null,       // Primarily used for updating records in existing databases when applying changesets that add columns. Need to add changeset functionality before implementing this. 
                    ValueNumeric = null,    // Primarily used for updating records in existing databases when applying changesets that add columns. Need to add changeset functionality before implementing this. 
                    Constraints = MapConstraints(prop, tableName, columnName)
                };

                var defaultAttr = prop.GetCustomAttribute<DefaultValueAttribute>();
                if (defaultAttr?.Value != null)
                {
                    switch (defaultAttr.Value)
                    {
                        case string s:
                            column.DefaultValue = s;
                            break;
                        case bool b:
                            column.DefaultValueBoolean = b;
                            break;
                        case DateTime d:
                            column.DefaultValueDate = d;
                            break;
                        case decimal or int or float or double:
                            column.DefaultValueNumeric = defaultAttr.Value;
                            break;
                        default:
                            column.DefaultValue = defaultAttr.Value;
                            break;
                    }
                    column.DefaultValueConstraintName = GetDefaultConstraintName(tableName, columnName);
                }

                var defaultValueComputedAttr = prop.GetCustomAttribute<DefaultValueComputedAttribute>();
                if (defaultValueComputedAttr?.Expression != null)
                {
                    column.DefaultValueComputed = defaultValueComputedAttr.Expression;
                    column.DefaultValueConstraintName = GetDefaultConstraintName(tableName, columnName);
                }
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

        private AutoIncrementAttribute? GetAutoIncrementAttribute(
            PropertyInfo prop,
            DatabaseGeneratedAttribute? databaseGeneratedAttr)
        {
            var autoIncrementAttr = prop.GetCustomAttribute<AutoIncrementAttribute>();
            if (autoIncrementAttr == null)
            {
                if (databaseGeneratedAttr?.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                {
                    autoIncrementAttr = new AutoIncrementAttribute();
                }
            }

            return autoIncrementAttr;
        }

        private LiquibaseConstraints MapConstraints(PropertyInfo prop, string tableName, string columnName)
        {
            var checkAttr = prop.GetCustomAttribute<CheckAttribute>();
            var deleteCascadeAttr = prop.GetCustomAttribute<DeleteCascadeAttribute>();
            var requiredAttr = prop.GetCustomAttribute<RequiredAttribute>();
            var primaryKeyAttr = GetPrimaryKeyAttribute(prop);
            var uniqueAttr = prop.GetCustomAttribute<UniqueAttribute>();

            var constraints = new LiquibaseConstraints()
            {
                CheckConstraint = checkAttr?.Expression,
                DeleteCascade = deleteCascadeAttr != null,
                Deferrable = checkAttr?.Deferrable,
                InitiallyDeferred = checkAttr?.InitiallyDeferred,
                NotNullConstraintName = requiredAttr == null
                    ? null
                    : GetNotNullConstraintName(tableName, columnName),
                Nullable = requiredAttr != null,
                PrimaryKey = primaryKeyAttr != null,
                PrimaryKeyName = primaryKeyAttr == null
                    ? null
                    : GetPrimaryKeyConstraintName(tableName, columnName),
                PrimaryKeyTablespace = primaryKeyAttr?.Tablespace,
                Unique = uniqueAttr != null,
                UniqueConstraintName = uniqueAttr == null
                    ? null
                    : GetUniqueConstraintName(tableName, columnName),
                ReferencedColumnNames = null,
                ReferencedTableName = null,
                ReferencedTableSchemaName = null,
                ValidateForeignKey = null,
                ValidateNullable = null,
                ValidatePrimaryKey = null,
                ValidateUnique = null
            };



            return constraints;
        }

        private PrimaryKeyAttribute GetPrimaryKeyAttribute(PropertyInfo prop)
        {
            var primaryKeyAttr = prop.GetCustomAttribute<PrimaryKeyAttribute>();
            if (primaryKeyAttr == null)
            {
                var keyAttr = prop.GetCustomAttribute<KeyAttribute>();
                if (keyAttr != null)
                {
                    primaryKeyAttr = new PrimaryKeyAttribute();
                }
            }
            return primaryKeyAttr;
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

        private string GetCheckConstraintName(string table, string column)
        {
            return GetConstraintName(_checkConstraintPrefix, table, column);
        }

        private string GetDefaultConstraintName(string table, string column)
        {
            return GetConstraintName(_defaultConstraintPrefix, table, column);
        }

        private string GetForeignKeyConstraintName(string table, string column)
        {
            return GetConstraintName(_foreignKeyConstraintPrefix, table, column);
        }

        private string GetIndexConstraintName(string table, string column)
        {
            return GetConstraintName(_indexConstraintPrefix, table, column);
        }

        private string GetNotNullConstraintName(string table, string column)
        {
            return GetConstraintName(_notNullConstraintPrefix, table, column);
        }

        private string GetPrimaryKeyConstraintName(string table, string column)
        {
            return GetConstraintName(_primaryKeyConstraintPrefix, table, column);
        }

        private string GetUniqueConstraintName(string table, string column)
        {
            return GetConstraintName(_uniqueConstraintPrefix, table, column);
        }

        private string GetConstraintName(string prefix, string table, string column)
        {
            return $"{prefix}{table}_{column}";
        }

    }
}

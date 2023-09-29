using Corely.Shared.Mappers.Liquibase.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using CorelyForeignKeyAttribute = Corely.Shared.Attributes.Db.ForeignKeyAttribute;
using SystemForeignKeyAttribute = System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute;

namespace Corely.Shared.Mappers.Liquibase.EntityMappers
{
    internal class EntityToLiquibaseTableMapper : ILiquibaseTableMapper
    {
        private readonly Type _entity;

        public EntityToLiquibaseTableMapper(Type entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            _entity = entity;
        }
        public LiquibaseCreateTable Map()
        {
            var tableName = _entity.GetCustomAttribute<TableAttribute>()?.Name ?? _entity.Name;

            var createTable = new LiquibaseCreateTable
            {
                TableName = tableName,
                Columns = MapColumns(_entity, tableName)
            };

            return createTable;
        }

        private List<LiquibaseColumn> MapColumns(Type entity, string tableName)
        {
            List<LiquibaseColumn> columns = new();
            List<CorelyForeignKeyAttribute> foreignKeys = new();

            foreach (var prop in entity.GetProperties())
            {
                if (MapColumn(prop, tableName, out var column))
                {
                    columns.Add(column);
                }
                else if (MapNavigationPropertyForeignKey(prop, out CorelyForeignKeyAttribute? foreignKey)
                    && foreignKey != null)
                {
                    foreignKeys.Add(foreignKey);
                }
            }

            return columns;
        }

        private bool MapColumn(PropertyInfo prop, string tableName, out LiquibaseColumn column)
        {
            column = null;

            var columnAttr = GetColumnAttribute(prop);

            if (columnAttr.TypeName != "UNKNOWN_TYPE")
            {
                var columnMapper = new EntityToLiquibaseColumnMapper(prop, columnAttr, tableName);
                column = columnMapper.Map();
            }

            return column != null;
        }

        private ColumnAttribute GetColumnAttribute(PropertyInfo prop)
        {
            var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();

            columnAttr ??= new ColumnAttribute(prop.Name);
            columnAttr.TypeName ??= MapSqlType(prop);

            return columnAttr;
        }

        private string MapSqlType(PropertyInfo prop)
        {
            Type underlyingType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
            var maxLength = prop.GetCustomAttribute<MaxLengthAttribute>()?.Length;

            return underlyingType switch
            {
                Type t when t == typeof(int) => "INT",
                Type t when t == typeof(string) => maxLength == null ? "VARCHAR" : $"VARCHAR({maxLength})",
                Type t when t == typeof(bool) => "BOOLEAN",
                Type t when t == typeof(DateTime) => "DATETIME",
                Type t when t == typeof(float) => "FLOAT",
                Type t when t == typeof(double) => "DOUBLE",
                Type t when t == typeof(decimal) => "DECIMAL",
                _ => "UNKNOWN_TYPE",
            };
        }

        private bool MapNavigationPropertyForeignKey(PropertyInfo prop, out CorelyForeignKeyAttribute? corelyFkAttr)
        {
            corelyFkAttr = prop.GetCustomAttribute<CorelyForeignKeyAttribute>();

            if (corelyFkAttr == null)
            {
                var systemFkAttr = prop.GetCustomAttribute<SystemForeignKeyAttribute>();
                if (systemFkAttr != null)
                {
                    var entity = prop.PropertyType;
                    var table = entity.GetCustomAttribute<TableAttribute>()?.Name ?? entity.Name;

                    if (!string.IsNullOrWhiteSpace(table))
                    {
                        corelyFkAttr = new CorelyForeignKeyAttribute(table, systemFkAttr.Name);
                    }
                }
            }

            return corelyFkAttr != null;
        }
    }
}

using Corely.Shared.Extensions;
using Corely.Shared.Mappers.Liquibase.EntityMappers.Finders;
using Corely.Shared.Mappers.Liquibase.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Corely.Shared.Mappers.Liquibase.EntityMappers
{
    internal class EntityToLiquibaseTableMapper : ILiquibaseTableMapper
    {
        private readonly Type _entity;
        private readonly IEntityForeignKeyFinder _foreignKeyFinder;

        public EntityToLiquibaseTableMapper(
            Type entity,
            IEntityForeignKeyFinder foreignKeyFinder)
        {
            _entity = entity.ThrowIfNull(nameof(entity));
            _foreignKeyFinder = foreignKeyFinder.ThrowIfNull(nameof(foreignKeyFinder));
        }
        public LiquibaseCreateTable Map()
        {
            var tableName = _entity.GetCustomAttribute<TableAttribute>()?.Name ?? _entity.Name;

            var createTable = new LiquibaseCreateTable
            {
                TableName = tableName,
                Columns = MapColumns(tableName)
            };

            return createTable;
        }

        private List<LiquibaseColumn> MapColumns(string tableName)
        {
            List<LiquibaseColumn> columns = new();

            foreach (var prop in _entity.GetProperties())
            {
                if (MapColumn(prop, tableName, out var column))
                {
                    columns.Add(column);
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
                var constraintNamePostfix = $"{tableName}_{columnAttr.Name}";

                var constraintMapper = new EntityToLiquibaseConstraintMapper(
                    prop,
                    constraintNamePostfix,
                    _foreignKeyFinder);

                var columnMapper = new EntityToLiquibaseColumnMapper(
                    prop,
                    columnAttr,
                    constraintNamePostfix,
                    constraintMapper);

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
    }
}

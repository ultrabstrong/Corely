using Corely.Shared.Attributes.Db;
using Corely.Shared.Extensions;
using Corely.Shared.Mappers.Liquibase.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Corely.Shared.Mappers.Liquibase.EntityMappers
{
    internal class EntityToLiquibaseColumnMapper : ILiquibaseColumnMapper
    {
        private const string _defaultConstraintPrefix = "DF_";

        private readonly PropertyInfo _prop;
        private readonly ColumnAttribute _columnAttr;
        private readonly string _constraintNamePostfix;
        private readonly ILiquibaseConstraintMapper _constraintMapper;

        private LiquibaseColumn _column;

        public EntityToLiquibaseColumnMapper(
            PropertyInfo prop,
            ColumnAttribute columnAttr,
            string constraintNamePostfix,
            ILiquibaseConstraintMapper constraintMapper)
        {
            _prop = prop.ThrowIfNull(nameof(prop));

            _columnAttr = columnAttr.ThrowIfNull(nameof(columnAttr));
            ArgumentNullException.ThrowIfNull(columnAttr.Name, nameof(columnAttr.Name));
            ArgumentNullException.ThrowIfNull(columnAttr.TypeName, nameof(columnAttr.TypeName));

            _constraintNamePostfix = constraintNamePostfix
                .ThrowIfNullOrWhiteSpace(nameof(constraintNamePostfix));

            _constraintMapper = constraintMapper
                .ThrowIfNull(nameof(constraintMapper));
        }

        public LiquibaseColumn Map()
        {
            _column = new LiquibaseColumn();

            MapColumn();
            MapColumnPlacement();
            MapDescending();
            MapRemarks();

            var databaseGeneratedAttr = _prop.GetCustomAttribute<DatabaseGeneratedAttribute>();
            MapDatabaseGenerated(databaseGeneratedAttr);
            MapAutoIncrement(databaseGeneratedAttr);

            MapDefault();
            MapDefaultComputed();
            MapConstraints();

            MapEncoding();
            MapValues();

            return _column;
        }

        private bool? HasValueTrueOrNull(object? value)
        {
            if (value == null) { return null; }
            return true;
        }

        private void MapColumn()
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            // Compiler doesn't realize these are null checked in constructor
            _column.Name = _columnAttr.Name;
            _column.Type = _columnAttr.TypeName;
#pragma warning restore CS8601 // Possible null reference assignment.
            _column.Position = _columnAttr.Order > -1 ? _columnAttr.Order : null;
        }

        private void MapColumnPlacement()
        {
            var columnPlacementAttr = _prop.GetCustomAttribute<ColumnPlacementAttribute>();
            _column.AfterColumn = columnPlacementAttr?.AfterColumn;
            _column.BeforeColumn = columnPlacementAttr?.BeforeColumn;
        }

        private void MapDatabaseGenerated(DatabaseGeneratedAttribute? databaseGeneratedAttr)
        {
            // Do this so it doesn't accidentally get set to 'false' when it should be null
            if (databaseGeneratedAttr != null)
            {
                _column.Computed = databaseGeneratedAttr.DatabaseGeneratedOption == DatabaseGeneratedOption.Computed;
            }
        }

        private void MapAutoIncrement(DatabaseGeneratedAttribute? databaseGeneratedAttr)
        {
            var autoIncrementAttr = GetAutoIncrementAttribute(databaseGeneratedAttr);
            _column.AutoIncrement = HasValueTrueOrNull(autoIncrementAttr);
            _column.StartWith = autoIncrementAttr?.StartWith;
            _column.IncrementBy = autoIncrementAttr?.IncrementBy;
        }

        private AutoIncrementAttribute? GetAutoIncrementAttribute(DatabaseGeneratedAttribute? databaseGeneratedAttr)
        {
            var autoIncrementAttr = _prop.GetCustomAttribute<AutoIncrementAttribute>();
            if (autoIncrementAttr == null)
            {
                if (databaseGeneratedAttr?.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                {
                    autoIncrementAttr = new AutoIncrementAttribute();
                }
            }

            return autoIncrementAttr;
        }

        private void MapDescending()
        {
            var descendingIndexAttr = _prop.GetCustomAttribute<DescendingIndexAttribute>();
            _column.Descending = HasValueTrueOrNull(descendingIndexAttr);
        }

        private void MapRemarks()
        {
            var remarksAttr = _prop.GetCustomAttribute<RemarksAttribute>();
            _column.Remarks = remarksAttr?.Remarks;
        }

        private void MapConstraints()
        {
            _column.Constraints = _constraintMapper.Map();
        }

        private void MapDefault()
        {
            var defaultAttr = _prop.GetCustomAttribute<DefaultValueAttribute>();
            if (defaultAttr?.Value != null)
            {
                switch (defaultAttr.Value)
                {
                    case string s:
                        _column.DefaultValue = s;
                        break;
                    case bool b:
                        _column.DefaultValueBoolean = b;
                        break;
                    case DateTime d:
                        _column.DefaultValueDate = d;
                        break;
                    case decimal or int or float or double:
                        _column.DefaultValueNumeric = defaultAttr.Value;
                        break;
                    default:
                        _column.DefaultValue = defaultAttr.Value;
                        break;
                }
                _column.DefaultValueConstraintName = $"{_defaultConstraintPrefix}{_constraintNamePostfix}";
            }
        }

        private void MapDefaultComputed()
        {
            var defaultValueComputedAttr = _prop.GetCustomAttribute<DefaultValueComputedAttribute>();
            if (defaultValueComputedAttr?.Expression != null)
            {
                _column.DefaultValueComputed = defaultValueComputedAttr.Expression;
                _column.DefaultValueConstraintName = $"{_defaultConstraintPrefix}{_constraintNamePostfix}";
            }
        }

        private void MapEncoding()
        {
            _column.Encoding = null;        // Primarily used for updating records in existing databases when applying changesets that add columns. Need to add changeset functionality before implementing this. 
        }

        private void MapValues()
        {

            _column.Value = null;           // Primarily used for updating records in existing databases when applying changesets that add columns. Need to add changeset functionality before implementing this. 
            _column.ValueBlobFile = null;   // Primarily used for updating records in existing databases when applying changesets that add columns. Need to add changeset functionality before implementing this. 
            _column.ValueBoolean = null;    // Primarily used for updating records in existing databases when applying changesets that add columns. Need to add changeset functionality before implementing this. 
            _column.ValueClobFile = null;   // Primarily used for updating records in existing databases when applying changesets that add columns. Need to add changeset functionality before implementing this. 
            _column.ValueComputed = null;   // Primarily used for updating records in existing databases when applying changesets that add columns. Need to add changeset functionality before implementing this. 
            _column.ValueDate = null;       // Primarily used for updating records in existing databases when applying changesets that add columns. Need to add changeset functionality before implementing this. 
            _column.ValueNumeric = null;    // Primarily used for updating records in existing databases when applying changesets that add columns. Need to add changeset functionality before implementing this. 
        }
    }
}

using Corely.Shared.Attributes.Db;
using Corely.Shared.Mappers.Liquibase.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Corely.Shared.Mappers.Liquibase.EntityMappers
{
    internal class EntityToLiquibaseConstraintMapper : ILiquibaseConstraintMapper
    {
        private const string
            _foreignKeyConstraintPrefix = "FK_",
            _notNullConstraintPrefix = "NN_",
            _primaryKeyConstraintPrefix = "PK_",
            _uniqueConstraintPrefix = "UQ_";

        private readonly PropertyInfo _prop;
        private readonly string _constraintNamePostifx;

        private LiquibaseConstraints _constraints;

        public EntityToLiquibaseConstraintMapper(PropertyInfo prop, string constraintNamePostifx)
        {
            ArgumentNullException.ThrowIfNull(prop, nameof(prop));

            _prop = prop;
            _constraintNamePostifx = constraintNamePostifx;
        }

        public LiquibaseConstraints? Map()
        {
            _constraints = new LiquibaseConstraints();

            MapCheck();
            MapDeleteCascade();
            MapNullable();
            MapPrimaryKey();
            MapUnique();

            return HasAnyConstraints() ? _constraints : null;
        }

        private bool? HasValueTrueOrNull(object? value)
        {
            if (value == null) { return null; }
            return true;
        }

        private void MapCheck()
        {
            var checkAttr = _prop.GetCustomAttribute<CheckAttribute>();
            _constraints.CheckConstraint = checkAttr?.Expression;
            _constraints.Deferrable = checkAttr?.Deferrable;
            _constraints.InitiallyDeferred = checkAttr?.InitiallyDeferred;
        }

        private void MapDeleteCascade()
        {
            var deleteCascadeAttr = _prop.GetCustomAttribute<DeleteCascadeAttribute>();
            _constraints.DeleteCascade = HasValueTrueOrNull(deleteCascadeAttr);
        }

        private void MapNullable()
        {
            var requiredAttr = _prop.GetCustomAttribute<RequiredAttribute>();
            _constraints.Nullable = !HasValueTrueOrNull(requiredAttr);
            _constraints.NotNullConstraintName = requiredAttr == null
                ? null
                : $"{_notNullConstraintPrefix}{_constraintNamePostifx}";

            var validateRequiredAttr = _prop.GetCustomAttribute<ValidateRequiredAttribute>();
            _constraints.ValidateNullable = HasValueTrueOrNull(validateRequiredAttr);
        }

        private void MapPrimaryKey()
        {
            var primaryKeyAttr = GetPrimaryKeyConstraint();
            _constraints.PrimaryKey = HasValueTrueOrNull(primaryKeyAttr);
            _constraints.ValidatePrimaryKey = primaryKeyAttr?.Validate;
            _constraints.PrimaryKeyTablespace = primaryKeyAttr?.Tablespace;
            _constraints.PrimaryKeyName = primaryKeyAttr == null
                ? null
                : $"{_primaryKeyConstraintPrefix}{_constraintNamePostifx}";
        }

        private PrimaryKeyAttribute? GetPrimaryKeyConstraint()
        {
            var primaryKeyAttr = _prop.GetCustomAttribute<PrimaryKeyAttribute>();
            if (primaryKeyAttr == null)
            {
                var keyAttr = _prop.GetCustomAttribute<KeyAttribute>();
                if (keyAttr != null)
                {
                    primaryKeyAttr = new PrimaryKeyAttribute();
                }
            }
            return primaryKeyAttr;
        }

        private void MapUnique()
        {
            var uniqueAttr = _prop.GetCustomAttribute<UniqueAttribute>();
            _constraints.Unique = HasValueTrueOrNull(uniqueAttr);
            _constraints.ValidateUnique = uniqueAttr?.Validate;
            _constraints.UniqueConstraintName = uniqueAttr == null
                ? null
                : $"{_uniqueConstraintPrefix}{_constraintNamePostifx}";
        }

        private bool HasAnyConstraints()
        {
            return _constraints.CheckConstraint != null
                || _constraints.DeleteCascade != null
                || _constraints.Deferrable != null
                || _constraints.ForeignKeyName != null
                || _constraints.InitiallyDeferred != null
                || _constraints.NotNullConstraintName != null
                || _constraints.Nullable != null
                || _constraints.PrimaryKey != null
                || _constraints.PrimaryKeyName != null
                || _constraints.PrimaryKeyTablespace != null
                || _constraints.Unique != null
                || _constraints.UniqueConstraintName != null
                || _constraints.References != null
                || _constraints.ReferencedColumnNames != null
                || _constraints.ReferencedTableName != null
                || _constraints.ReferencedTableSchemaName != null
                || _constraints.ValidateForeignKey != null
                || _constraints.ValidateNullable != null
                || _constraints.ValidatePrimaryKey != null
                || _constraints.ValidateUnique != null;

        }
    }
}

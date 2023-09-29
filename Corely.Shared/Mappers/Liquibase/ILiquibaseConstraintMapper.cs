using Corely.Shared.Mappers.Liquibase.Models;

namespace Corely.Shared.Mappers.Liquibase
{
    public interface ILiquibaseConstraintMapper
    {
        public LiquibaseConstraints? Map();
    }
}

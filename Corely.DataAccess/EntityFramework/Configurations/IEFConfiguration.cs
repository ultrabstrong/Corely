using Microsoft.EntityFrameworkCore;

namespace Corely.DataAccess.EntityFramework.Configurations
{
    public interface IEFConfiguration
    {
        void Configure(DbContextOptionsBuilder optionsBuilder);

        IEFDbTypes GetDbTypes();
    }
}

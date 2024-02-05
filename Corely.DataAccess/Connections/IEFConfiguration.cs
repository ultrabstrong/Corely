using Microsoft.EntityFrameworkCore;

namespace Corely.DataAccess.Connections
{
    public interface IEFConfiguration
    {
        void Configure(DbContextOptionsBuilder optionsBuilder);
    }
}

using Corely.DataAccess.EntityFramework.Configurations;

namespace Corely.DataAccess.EntityFramework
{
    public record EFConnection(IEFConfiguration Configuration)
    {
    }
}

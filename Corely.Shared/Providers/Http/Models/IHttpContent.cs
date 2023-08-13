namespace Corely.Shared.Providers.Http.Models
{
    public interface IHttpContent<T>
    {
        T Content { get; }
    }
}

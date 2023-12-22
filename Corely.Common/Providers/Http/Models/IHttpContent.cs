namespace Corely.Common.Providers.Http.Models
{
    public interface IHttpContent<T>
    {
        T Content { get; }
    }
}

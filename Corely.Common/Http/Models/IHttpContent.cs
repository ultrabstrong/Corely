namespace Corely.Common.Http.Models
{
    public interface IHttpContent<T>
    {
        T Content { get; }
    }
}

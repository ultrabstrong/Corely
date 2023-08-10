namespace Corely.Shared.Extensions
{
    public static class GenericExtensions
    {
        public static T ThrowIfNull<T>(this T obj)
        {
            ArgumentNullException.ThrowIfNull(obj);
            return obj;
        }
    }
}

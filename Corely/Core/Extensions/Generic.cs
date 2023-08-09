namespace Corely.Core.Extensions
{
    public static class Generic
    {
        public static T ThrowIfNull<T>(this T obj)
        {
            ArgumentNullException.ThrowIfNull(obj);
            return obj;
        }
    }
}

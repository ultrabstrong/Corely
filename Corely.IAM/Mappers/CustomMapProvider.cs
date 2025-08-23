using System.Reflection;

namespace Corely.IAM.Mappers;

internal sealed class CustomMapProvider : IMapProvider
{
    private static readonly Dictionary<(Type, Type), Func<object, object>> _mapperCache = new();
    private static readonly object _lock = new();

    public TDestination? MapTo<TDestination>(object? source)
    {
        if (source == null)
            return default;

        var sourceType = source.GetType();
        var destinationType = typeof(TDestination);

        var mapperFunction = GetOrCreateMapper(sourceType, destinationType);
        if (mapperFunction == null)
            throw new InvalidOperationException($"No mapping found from {sourceType.Name} to {destinationType.Name}");

        return (TDestination)mapperFunction(source);
    }

    private static Func<object, object>? GetOrCreateMapper(Type sourceType, Type destinationType)
    {
        var key = (sourceType, destinationType);
        
        lock (_lock)
        {
            if (_mapperCache.TryGetValue(key, out var cachedMapper))
                return cachedMapper;

            var mapper = FindMapper(sourceType, destinationType);
            if (mapper != null)
                _mapperCache[key] = mapper;

            return mapper;
        }
    }

    private static Func<object, object>? FindMapper(Type sourceType, Type destinationType)
    {
        // Search for static mapper classes in the assembly
        var assembly = Assembly.GetExecutingAssembly();
        var mapperClasses = assembly.GetTypes()
            .Where(t => t.IsClass && t.IsSealed && t.IsAbstract && // static class
                       t.Name.EndsWith("Mapper") &&
                       t.Namespace?.Contains("Mappers") == true)
            .ToList();

        foreach (var mapperClass in mapperClasses)
        {
            // Look for methods that match the mapping pattern
            var methods = mapperClass.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.GetParameters().Length == 1 &&
                           m.GetParameters()[0].ParameterType == sourceType &&
                           m.ReturnType == destinationType)
                .ToList();

            if (methods.Count > 0)
            {
                var method = methods.First();
                return source => method.Invoke(null, new[] { source })!;
            }
        }

        return null;
    }
}
using AutoMapper;
using System.Text.Json;

namespace Corely.Domain.Mappers.AutoMapper.ValueConverters
{
    internal class JsonifyListValueConverter<T> : IValueConverter<IEnumerable<T>, List<string>?>
    {
        public List<string>? Convert(IEnumerable<T> sourceMember, ResolutionContext context)
        {
            if (sourceMember == null) { return null; }
            return sourceMember.Select(s => JsonSerializer.Serialize(s)).ToList();
        }
    }
}

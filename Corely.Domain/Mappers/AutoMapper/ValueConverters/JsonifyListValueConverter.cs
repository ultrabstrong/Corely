using AutoMapper;
using System.Text.Json;

namespace Corely.Domain.Mappers.AutoMapper.ValueConverters
{
    public class JsonifyListValueConverter<T> : IValueConverter<IEnumerable<T>, List<string>>
    {
        public List<string> Convert(IEnumerable<T> sourceMember, ResolutionContext context)
        {
            return sourceMember.Select(s => JsonSerializer.Serialize(s)).ToList();
        }
    }
}

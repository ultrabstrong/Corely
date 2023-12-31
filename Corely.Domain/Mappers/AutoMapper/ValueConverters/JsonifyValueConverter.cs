using AutoMapper;
using System.Text.Json;

namespace Corely.Domain.Mappers.AutoMapper.ValueConverters
{
    internal class JsonifyValueConverter<T> : IValueConverter<T, string>
    {
        public string Convert(T sourceMember, ResolutionContext context)
        {
            return JsonSerializer.Serialize(sourceMember);
        }
    }
}

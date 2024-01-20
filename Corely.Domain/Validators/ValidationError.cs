namespace Corely.Domain.Validators
{
    public class ValidationError
    {
        public string Message { get; init; } = null!;
        public string PropertyName { get; init; } = null!;
        public object? PropertyValue { get; init; } = null;
    }
}

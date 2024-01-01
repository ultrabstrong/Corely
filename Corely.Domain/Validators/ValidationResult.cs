using Corely.Domain.Exceptions;

namespace Corely.Domain.Validators
{
    public class ValidationResult
    {
        public string Message { get; init; } = null!;
        public List<string>? Errors { get; init; }
        public bool IsValid => Errors == null || Errors.Count == 0;

        public void ThrowIfInvalid()
        {
            if (!IsValid)
            {
                throw new ValidationException(Message)
                {
                    ValidationResult = this
                };
            }
        }
    }
}

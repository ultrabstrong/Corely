using Corely.Domain.Exceptions;

namespace Corely.Domain.Validators
{
    public class ValidationResult
    {
        public string Message { get; init; }
        public List<string> Errors { get; init; } = new();
        public bool IsValid => Errors?.Count == 0;

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

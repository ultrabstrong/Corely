namespace Corely.Domain.Validators
{
    public interface IValidationProvider
    {
        public ValidationResult Validate<T>(T model);
    }
}

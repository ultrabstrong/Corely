namespace Corely.IAM.Validators
{
    public interface IValidationProvider
    {
        public ValidationResult Validate<T>(T model);

        public void ThrowIfInvalid<T>(T model);
    }
}

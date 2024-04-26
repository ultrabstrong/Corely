using FluentValidation;

namespace Corely.IAM.Validators.FluentValidators
{
    public interface IFluentValidatorFactory
    {
        IValidator<T> GetValidator<T>();
    }
}

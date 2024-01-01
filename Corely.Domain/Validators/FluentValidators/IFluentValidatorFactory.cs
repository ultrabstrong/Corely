using FluentValidation;

namespace Corely.Domain.Validators.FluentValidators
{
    public interface IFluentValidatorFactory
    {
        IValidator<T> GetValidator<T>();
    }
}

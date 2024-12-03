using FluentValidation;

namespace Corely.IAM.Validators.FluentValidators;

internal sealed class FluentValidatorFactory : IFluentValidatorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public FluentValidatorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IValidator<T> GetValidator<T>()
    {
        if (_serviceProvider.GetService(typeof(IValidator<T>)) is not IValidator<T> validator)
        {
            throw new InvalidOperationException($"No validator found for type {typeof(T).Name}");
        }

        return validator;
    }
}

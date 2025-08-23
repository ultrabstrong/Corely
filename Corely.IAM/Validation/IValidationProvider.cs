namespace Corely.IAM.Validation;

public interface IValidationProvider
{
    void ValidateAndThrow<T>(T obj);
}
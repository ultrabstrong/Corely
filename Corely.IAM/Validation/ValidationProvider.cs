using System.ComponentModel.DataAnnotations;

namespace Corely.IAM.Validation;

internal class ValidationProvider : IValidationProvider
{
    public void ValidateAndThrow<T>(T obj)
    {
        if (obj == null)
        {
            throw new ValidationException("Object cannot be null");
        }

        var context = new ValidationContext(obj);
        var results = new List<ValidationResult>();
        
        if (!Validator.TryValidateObject(obj, context, results, true))
        {
            var errorMessage = string.Join("; ", results.Select(r => r.ErrorMessage));
            throw new ValidationException(errorMessage);
        }
    }
}
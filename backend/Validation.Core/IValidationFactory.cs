namespace Validation.Core;

public interface IValidationFactory
{
    void ValidateAndThrow<T>(T dto);
}
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;   

namespace Validation.Core.Validators;

public class ValidationFactory : IValidationFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void ValidateAndThrow<T>(T dto)
    {
        IValidator<T>? validator = _serviceProvider.GetService<IValidator<T>>();
        ProcessValidation(validator, dto);
    }

    private static void ProcessValidation<T>(IValidator<T>? validator, T dto)
    {
        validator?.ValidateAndThrow(dto);
    }
}
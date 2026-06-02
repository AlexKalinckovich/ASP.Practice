using ContactBackend.Application.Core.Validators;
using FluentValidation;
using Model.Core.DTOs;

namespace Validation.Core.Validators;

public class ContactCreateDtoValidator : AbstractValidator<ContactCreateDto>
{
    public ContactCreateDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .ApplyStringLengthRules(150);

        RuleFor(dto => dto.MobilePhone)
            .NotEmpty()
            .ApplyStringLengthRules(20);

        RuleFor(dto => dto.JobTitle)
            .ApplyStringLengthRules(100);

        RuleFor(dto => dto.BirthDate)
            .NotEmpty()
            .ApplyPastDateRules();
    }
}
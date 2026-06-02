using FluentValidation;
using Model.Core.DTOs;

namespace ContactBackend.Application.Core.Validators;

public class ContactUpdateDtoValidator : AbstractValidator<ContactUpdateDto>
{
    public ContactUpdateDtoValidator()
    {
        RuleFor(dto => dto.Id)
            .GreaterThan(0);

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
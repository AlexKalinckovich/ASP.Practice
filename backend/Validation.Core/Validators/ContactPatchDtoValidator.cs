using FluentValidation;
using Model.Core.DTOs;

namespace Validation.Core.Validators;

public class ContactPatchDtoValidator : AbstractValidator<ContactPatchDto>
{
    public ContactPatchDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .ApplyStringLengthRules(150)
            .When(dto => dto.Name != null);

        RuleFor(dto => dto.MobilePhone)
            .ApplyMobilePhoneRules()
            .ApplyStringLengthRules(20)
            .When(dto => dto.MobilePhone != null);

        RuleFor(dto => dto.JobTitle)
            .ApplyJobTitleRules()
            .ApplyStringLengthRules(100)
            .When(dto => dto.JobTitle != null);

        RuleFor(dto => dto.BirthDate)
            .ApplyNullablePastDateRules()
            .ApplyNullableAdultAgeRules()
            .When(dto => dto.BirthDate.HasValue);
    }
}
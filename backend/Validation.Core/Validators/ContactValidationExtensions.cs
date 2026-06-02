using System.Text.RegularExpressions;
using FluentValidation;

namespace Validation.Core.Validators;

public static partial class ContactValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ApplyStringLengthRules<T>(this IRuleBuilder<T, string> ruleBuilder, int maxLength)
    {
        return ruleBuilder.MaximumLength(maxLength);
    }

    public static IRuleBuilderOptions<T, DateTime> ApplyPastDateRules<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
    {
        return ruleBuilder.LessThan(DateTime.Today);
    }

    public static IRuleBuilderOptions<T, DateTime?> ApplyNullablePastDateRules<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
    {
        return ruleBuilder.LessThan(DateTime.Today);
    }

    public static IRuleBuilderOptions<T, string> ApplyMobilePhoneRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid international phone format.");
    }

    public static IRuleBuilderOptions<T, string> ApplyJobTitleRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(jobTitle => 
        {
            if (string.IsNullOrWhiteSpace(jobTitle))
            {
                return true;
            }
            return ValidJobTitleRegex().IsMatch(jobTitle);
        }).WithMessage("Job title contains invalid characters.");
    }

    public static IRuleBuilderOptions<T, DateTime> ApplyAdultAgeRules<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
    {
        return ruleBuilder.Must(birthDate => 
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }
            return age >= 18;
        }).WithMessage("Contact must be at least 18 years old.");
    }

    public static IRuleBuilderOptions<T, DateTime?> ApplyNullableAdultAgeRules<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
    {
        return ruleBuilder.Must(BeAtLeast18YearsOld())
            .WithMessage("Contact must be at least 18 years old.");
    }

    private static Func<DateTime?, bool> BeAtLeast18YearsOld()
    {
        return birthDate => 
        {
            if (!birthDate.HasValue)
            {
                return true;
            }
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Value.Year;
            if (birthDate.Value.Date > today.AddYears(-age))
            {
                age--;
            }
            return age >= 18;
        };
    }

    [GeneratedRegex(@"^[a-zA-Z0-9\s\-]+$")]
    private static partial Regex ValidJobTitleRegex();
}
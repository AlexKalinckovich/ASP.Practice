using FluentValidation;

namespace ContactBackend.Application.Core.Validators;

public static class ContactValidationExtensions
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
}
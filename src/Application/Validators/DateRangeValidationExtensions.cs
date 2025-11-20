using Domain.ValueObjects;
using FluentValidation;

namespace Application.Validators;

public static class DateRangeValidationExtensions
{
    private static readonly DateRangeValidator<DateRange> DateRangeValidator = new();

    private static readonly ContractDurationValidator ContractDurationValidator = new();

    public static IRuleBuilderOptions<T, DateRange> ValidDateRange<T>(
        this IRuleBuilder<T, DateRange> rule)
    {
        return rule
            .NotEmpty()
            .WithMessage("Contract duration is required.")
            .SetValidator(DateRangeValidator);
    }

    public static IRuleBuilderOptions<T, DateRange> ValidContractDuration<T>(
        this IRuleBuilder<T, DateRange> rule)
    {
        return rule
            .NotEmpty()
            .WithMessage("Contract duration is required.")
            .SetValidator(ContractDurationValidator);
    }
}

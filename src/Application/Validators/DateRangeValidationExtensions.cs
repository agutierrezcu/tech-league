using Domain.ValueObjects;
using FluentValidation;

namespace Application.Validators;

public static class DateRangeValidationExtensions
{
    private static readonly DateRangeValidator<DateRange> dateRangeValidator = new();

    private static readonly ContractDurationValidator contractDurationValidator = new();

    public static IRuleBuilderOptions<T, DateRange> ValidDateRange<T>(
        this IRuleBuilder<T, DateRange> rule)
    {
        return rule
            .NotEmpty()
            .WithMessage("Contract duration is required.")
            .SetValidator(dateRangeValidator);
    }

    public static IRuleBuilderOptions<T, DateRange> ValidContractDuration<T>(
       this IRuleBuilder<T, DateRange> rule)
    {
        return rule
            .NotEmpty()
            .WithMessage("Contract duration is required.")
            .SetValidator(contractDurationValidator);
    }
}


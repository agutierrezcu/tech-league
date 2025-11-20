using Domain.ValueObjects;
using FluentValidation;

namespace Application.Validators;

public class ContractDurationValidator : DateRangeValidator<DateRange>
{
    public ContractDurationValidator()
    {
        RuleFor(r => r.Start)
            .GreaterThan(DateTime.Today)
            .WithMessage("Start date must be later than today");
    }
}

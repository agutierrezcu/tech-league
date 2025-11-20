using Domain.ValueObjects;
using FluentValidation;

namespace Application.Validators;

public class DateRangeValidator<TValidatee> : AbstractValidator<TValidatee>
    where TValidatee : DateRange
{
    public DateRangeValidator()
    {
        RuleFor(r => r.Start)
            .NotEmpty()
            .WithMessage("Start date cannot be empty.");

        RuleFor(r => r.End)
            .NotEmpty()
            .WithMessage("End date cannot be empty.");

        RuleFor(r => r.Start)
            .LessThan(r => r.End)
            .WithMessage("Start date must be earlier than End date.");
    }
}

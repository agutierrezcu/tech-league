using Application.Validators;
using FluentValidation;

namespace Application.Clubs.Coaches.Hire;

internal sealed class HireCoachCommandValidator : AbstractValidator<HireCoachCommand>
{
    public HireCoachCommandValidator()
    {
        RuleFor(c => c.ClubId)
            .NotEmpty();

        RuleFor(c => c.CoachId)
            .NotEmpty();

        RuleFor(c => c.AnualSalary)
            .GreaterThanOrEqualTo(CoachContract.MinimumAnualSalary);

        RuleFor(c => c.ContractDuration)
            .ValidContractDuration();
    }
}

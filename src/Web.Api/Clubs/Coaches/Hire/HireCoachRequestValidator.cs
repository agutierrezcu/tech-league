using Application.Validators;
using FastEndpoints;
using FluentValidation;

namespace Web.Api.Clubs.Coaches.Hire;

internal sealed class HireCoachRequestValidator : Validator<HireCoachRequest>
{
    public HireCoachRequestValidator()
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

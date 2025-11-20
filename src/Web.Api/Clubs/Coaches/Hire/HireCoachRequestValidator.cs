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

        RuleFor(c => c.ContractDuration)
            .ValidContractDuration();

        RuleFor(c => c.AnualSalary)
            .GreaterThan(0m);
    }
}

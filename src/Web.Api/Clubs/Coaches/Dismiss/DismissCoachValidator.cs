using FastEndpoints;
using FluentValidation;

namespace Web.Api.Clubs.Coaches.Dismiss;

internal sealed class DismissCoachValidator : Validator<DismissCoachRequest>
{
    public DismissCoachValidator()
    {
        RuleFor(c => c.ClubId)
            .NotEmpty();

        RuleFor(c => c.CoachId)
            .NotEmpty();
    }
}

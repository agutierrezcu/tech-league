using FluentValidation;

namespace Application.Clubs.Coaches.Dismiss;

internal sealed class DismissCoachCommandValidator : AbstractValidator<DismissCoachCommand>
{
    public DismissCoachCommandValidator()
    {
        RuleFor(c => c.ClubId)
            .NotEmpty();

        RuleFor(c => c.CoachId)
            .NotEmpty();
    }
}

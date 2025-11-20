using FluentValidation;

namespace Application.Clubs.Players.RRelease;

internal sealed class ReleasePlayerCommandValidator : AbstractValidator<ReleasePlayerCommand>
{
    public ReleasePlayerCommandValidator()
    {
        RuleFor(c => c.ClubId)
            .NotEmpty();

        RuleFor(c => c.PlayerId)
            .NotEmpty();
    }
}

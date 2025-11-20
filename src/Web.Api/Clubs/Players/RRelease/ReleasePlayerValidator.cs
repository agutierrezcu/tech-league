using FastEndpoints;
using FluentValidation;

namespace Web.Api.Clubs.Players.RRelease;

internal sealed class ReleasePlayerValidator : Validator<ReleasePlayerRequest>
{
    public ReleasePlayerValidator()
    {
        RuleFor(c => c.ClubId)
            .NotEmpty();

        RuleFor(c => c.PlayerId)
            .NotEmpty();
    }
}

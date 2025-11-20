using FluentValidation;

namespace Application.Clubs.Players.Get;

internal sealed class GetPlayerByClubQueryValidator : AbstractValidator<GetPlayerByClubQuery>
{
    public GetPlayerByClubQueryValidator()
    {
        RuleFor(c => c.ClubId)
            .NotEmpty();

        RuleFor(c => c.PageIndex)
            .GreaterThanOrEqualTo(1);

        RuleFor(c => c.PageSize)
            .GreaterThanOrEqualTo(1);
    }
}

using FluentValidation;

namespace Application.Clubs.Get;

internal sealed class GetClubQueryValidator : AbstractValidator<GetClubQuery>
{
    public GetClubQueryValidator()
    {
        RuleFor(c => c.ClubId)
            .NotEmpty();
    }
}

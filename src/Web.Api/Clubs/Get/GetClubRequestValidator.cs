using FastEndpoints;
using FluentValidation;

namespace Web.Api.Clubs.Get;

internal sealed class GetClubRequestValidator : Validator<GetClubRequest>
{
    public GetClubRequestValidator()
    {
        RuleFor(c => c.ClubId)
            .NotEmpty();
    }
}

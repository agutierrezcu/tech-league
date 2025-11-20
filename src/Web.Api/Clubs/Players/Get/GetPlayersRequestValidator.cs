using FastEndpoints;
using FluentValidation;

namespace Web.Api.Clubs.Players.Get;

internal sealed class GetPlayersRequestValidator : Validator<GetPlayersRequest>
{
    public GetPlayersRequestValidator()
    {
        RuleFor(c => c.ClubId)
            .NotEmpty();

        RuleFor(c => c.PageIndex)
            .GreaterThanOrEqualTo(1);

        RuleFor(c => c.PageSize)
            .GreaterThanOrEqualTo(1);
    }
}

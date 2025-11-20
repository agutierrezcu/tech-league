using FastEndpoints;
using FluentValidation;

namespace Web.Api.Clubs.Finances.GetStatusByClub;

internal sealed class GetFinanceStatusByClubRequestValidator
    : Validator<GetFinanceStatusByClubRequest>
{
    public GetFinanceStatusByClubRequestValidator()
    {
        RuleFor(c => c.ClubId)
            .NotEmpty();
    }
}

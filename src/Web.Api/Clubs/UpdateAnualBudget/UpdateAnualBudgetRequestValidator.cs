using FastEndpoints;
using FluentValidation;

namespace Web.Api.Clubs.UpdateAnualBudget;

internal sealed class UpdateAnualBudgetRequestValidator : Validator<UpdateAnualBudgetRequest>
{
    public UpdateAnualBudgetRequestValidator()
    {
        RuleFor(c => c.ClubId)
            .NotEmpty();

        RuleFor(c => c.NewAnualBudget)
            .GreaterThanOrEqualTo(Club.MinimumAnualBudget);
    }
}

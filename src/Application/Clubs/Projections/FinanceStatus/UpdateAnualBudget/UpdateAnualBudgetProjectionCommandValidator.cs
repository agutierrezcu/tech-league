using FluentValidation;

namespace Application.Clubs.Projections.FinanceStatus.UpdateAnualBudget;

internal sealed class UpdateAnualBudgetProjectionCommandValidator :
    AbstractValidator<UpdateAnualBudgetProjectionCommand>
{
    public UpdateAnualBudgetProjectionCommandValidator()
    {
        RuleFor(c => c.ClubId)
            .NotEmpty();
    }
}

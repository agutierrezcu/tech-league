using FluentValidation;

namespace Application.Clubs.Projections.FinanceStatus.UpdateAnualBudget;

internal sealed class UpdateAnualBudgetProjectionCommandValidator : 
    AbstractValidator<UpdateAnualBudgetFinanceStatusProjectionCommand>
{
    public UpdateAnualBudgetProjectionCommandValidator()
    {
        RuleFor(c => c.ClubId)
            .NotEmpty();
    }
}

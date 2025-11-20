using FluentValidation;

namespace Application.Clubs.UpdateAnualBudget;

internal sealed class UpdateBudgetCommandValidator : AbstractValidator<UpdateAnualBudgetCommand>
{
    public UpdateBudgetCommandValidator()
    {
        RuleFor(c => c.ClubId)
            .NotEmpty();

        RuleFor(c => c.NewAnualBudget)
            .GreaterThanOrEqualTo(Club.MinimumAnualBudget);
    }
}

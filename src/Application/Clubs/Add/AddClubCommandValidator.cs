using FluentValidation;

namespace Application.Clubs.Add;

internal sealed class AddClubCommandValidator : AbstractValidator<AddClubCommand>
{
    public AddClubCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.ThreeLettersName)
            .NotEmpty()
            .Length(3);

        RuleFor(c => c.AnualBudget)
            .GreaterThanOrEqualTo(Club.MinimumAnualBudget);
    }
}

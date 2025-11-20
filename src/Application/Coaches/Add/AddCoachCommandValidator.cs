using FluentValidation;

namespace Application.Coaches.Add;

internal sealed class AddCoachCommandValidator : AbstractValidator<AddCoachCommand>
{
    public AddCoachCommandValidator()
    {
        RuleFor(c => c.FullName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.Experience)
            .GreaterThanOrEqualTo(Coach.MinimumExperience);
    }
}

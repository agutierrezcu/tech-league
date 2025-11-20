using FastEndpoints;
using FluentValidation;

namespace Web.Api.Coaches.Add;

internal sealed class AddCoachRequestValidator : Validator<AddCoachRequest>
{
    public AddCoachRequestValidator()
    {
        RuleFor(c => c.FullName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.Experience)
            .GreaterThanOrEqualTo(Coach.MinimumExperience);
    }
}

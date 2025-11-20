using FluentValidation;

namespace Application.Players.Add;

internal sealed class AddPlayerCommandValidator : AbstractValidator<AddPlayerCommand>
{
    public AddPlayerCommandValidator()
    {
        RuleFor(c => c.FullName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.NickName)
            .MaximumLength(20);
    }
}

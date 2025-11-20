using FastEndpoints;
using FluentValidation;

namespace Web.Api.Players.Add;

internal sealed class AddPlayerRequestValidator : Validator<AddPlayerRequest>
{
    public AddPlayerRequestValidator()
    {
        RuleFor(c => c.FullName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.NickName)
            .MaximumLength(20);
    }
}

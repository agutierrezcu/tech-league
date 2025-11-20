using Application.Validators;
using FluentValidation;

namespace Application.Clubs.Players.SignUp;

internal sealed class SignUpPlayerCommandValidator : AbstractValidator<SignUpPlayerCommand>
{
    public SignUpPlayerCommandValidator()
    {
        RuleFor(c => c.ClubId)
            .NotEmpty();

        RuleFor(c => c.PlayerId)
            .NotEmpty();

        RuleFor(c => c.AnualSalary)
            .GreaterThanOrEqualTo(PlayerContract.MinimumAnualSalary);

        RuleFor(c => c.ContractDuration)
            .ValidContractDuration();
    }
}

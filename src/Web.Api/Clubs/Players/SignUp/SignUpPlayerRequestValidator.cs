using Application.Validators;
using FastEndpoints;
using FluentValidation;

namespace Web.Api.Clubs.Players.SignUp;

internal sealed class SignUpPlayerRequestValidator : Validator<SignUpPlayerRequest>
{
    public SignUpPlayerRequestValidator()
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

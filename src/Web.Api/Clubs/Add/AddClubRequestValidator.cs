using FastEndpoints;
using FluentValidation;

namespace Web.Api.Clubs.Add;

internal sealed class AddClubRequestValidator : Validator<AddClubRequest>
{
    public AddClubRequestValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.ThreeLettersName)
            .NotEmpty()
            .Length(3);
    }
}

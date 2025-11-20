using FluentValidation;

namespace Application.Clubs.Projections.FinanceStatus.Create;

internal sealed class CreateFinanceStatusProjectionCommandValidator :
    AbstractValidator<CreateFinanceStatusProjectionCommand>
{
    public CreateFinanceStatusProjectionCommandValidator()
    {
        RuleFor(c => c.ClubId)
            .NotEmpty();
    }
}

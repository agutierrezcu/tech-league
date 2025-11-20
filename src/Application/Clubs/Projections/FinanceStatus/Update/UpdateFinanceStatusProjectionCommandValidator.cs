using FluentValidation;

namespace Application.Clubs.Projections.FinanceStatus.Update;

internal sealed class UpdateFinanceStatusProjectionCommandValidator :
    AbstractValidator<UpdateFinanceStatusProjectionCommand>
{
    public UpdateFinanceStatusProjectionCommandValidator()
    {
        RuleFor(c => c.ClubId)
            .NotEmpty();

        RuleFor(c => c.UpdateIn)
            .NotEmpty()
            .NotEqual(0)
            .WithMessage($"{nameof(UpdateFinanceStatusProjectionCommand.UpdateIn)} can not be 0.");

        RuleFor(c => c.ContractType)
            .IsInEnum();
    }
}

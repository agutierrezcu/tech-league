using SharedKernel;

namespace Application.Clubs.Projections.FinanceStatus;

public static class ClubFinanceStatusProjectionErrors
{
    public readonly static Error ClubNotFound =
        Error.NotFound("ClubNotFound", "Club with the given id was not found.");

    public readonly static Error FinanceStatusProjectionNotFound =
        Error.NotFound("FinanceStatusProjectionNotFound", "Finance status projection for given club id was not found.");

    public readonly static Error InsufficientAnualBudget =
       Error.Invalid("InsufficientAnualBudget", "Committed budget can not be above club's anual budget.");

    public readonly static Error InvalidContractType =
        Error.Invalid("InvalidContractType", $"Contract type must be either {nameof(ContractType.Player)} or {nameof(ContractType.Coach)}.");
}

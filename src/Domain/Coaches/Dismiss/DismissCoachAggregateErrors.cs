using SharedKernel;

namespace Domain.Coaches.Dismiss;

public static class DismissCoachAggregateErrors
{
    public readonly static Error ContractNotFound =
        Error.NotFound("ContractNotFound", "Contract linked club and coach was not found.");
}

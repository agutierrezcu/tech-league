using SharedKernel;

namespace Domain.Players.RRelease;

public static class ReleasePlayerAggregateErrors
{
    public readonly static Error ContractNotFound =
        Error.NotFound("ContractNotFound", 
            "Contract linked club and player was not found.");
}

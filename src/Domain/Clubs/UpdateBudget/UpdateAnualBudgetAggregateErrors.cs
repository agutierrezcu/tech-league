using SharedKernel;

namespace Domain.Clubs.UpdateBudget;

public static class UpdateAnualBudgetAggregateErrors
{
    public static readonly Error ClubNotFound =
        Error.NotFound("ClubNotFound", "Club with given id was not found.");

    public static readonly Error ValueBelowMinimumRequired =
        Error.Conflict("ValueBelowMinimumRequired", $"The given new value is bellow the minimum required: {Club.MinimumAnualBudget}.");

    public static readonly Error ValueBellowCommittedBudget =
        Error.Conflict("ValueBellowCommittedBudget", "The given new anual budget is below committed budget.");
}

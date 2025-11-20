using SharedKernel;

namespace Domain.Clubs.UpdateBudget;

public static class UpdateAnualBudgetAggregateErrors
{
    public readonly static Error ClubNotFound =
        Error.NotFound("ClubNotFound", "Club with given id was not found.");

    public readonly static Error ValueBelowMinimumRequired =
        Error.Conflict("ValueBelowMinimumRequired", $"The given new value is bellow the minimum required: {Club.LeagueMinimumBudgetRequired}.");

    public readonly static Error ValueBellowCommittedBudget =
        Error.Conflict("ValueBellowCommittedBudget", "The given new anual budget is below committed budget.");
}

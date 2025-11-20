using SharedKernel;

namespace Domain.Clubs.Add;

public static class ClubCreatorAggregateErrors
{
    public readonly static Error ClubAlreadyExist =
        Error.NotFound("ClubAlreadyExist", "Club with name o three letter names already exists.");

    public readonly static Error MinBudgetNotReached =
        Error.Invalid("MinBudgetNotReached",
            $"Club does not meet minimun budget requirement of {Club.LeagueMinimumBudgetRequired}.");
}

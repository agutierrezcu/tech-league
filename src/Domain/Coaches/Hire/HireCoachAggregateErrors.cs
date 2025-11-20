using SharedKernel;

namespace Domain.Coaches.Hire;

public static class HireCoachAggregateErrors
{
    public readonly static Error ClubNotFound =
        Error.NotFound("ClubNotFound", "Club with the given Id was not found.");

    public readonly static Error CoachNotFound =
        Error.NotFound("CoachNotFound", "Coach with the given Id was not found.");

    public readonly static Error CoachAlreadyHired =
        Error.Conflict("CoachAlreadyHired", "Coach is already working for the club.");

    public readonly static Error CoachWorkingForDifferentClub =
        Error.Conflict("CoachWorkingForDifferentClub", "Coach is working for different club.");

    public readonly static Error InsufficientRemainingBudget =
        Error.Conflict("InsufficientRemainingBudget", "Insufficient remaining budget to hire the Coach.");
}

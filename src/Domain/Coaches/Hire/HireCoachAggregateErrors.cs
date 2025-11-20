using SharedKernel;

namespace Domain.Coaches.Hire;

public static class HireCoachAggregateErrors
{
    public static readonly Error ClubNotFound =
        Error.NotFound("ClubNotFound", "Club with the given Id was not found.");

    public static readonly Error CoachNotFound =
        Error.NotFound("CoachNotFound", "Coach with the given Id was not found.");

    public static readonly Error CoachAlreadyHired =
        Error.Conflict("CoachAlreadyHired", "Coach is already working for the club.");

    public static readonly Error CoachWorkingForDifferentClub =
        Error.Conflict("CoachWorkingForDifferentClub", "Coach is working for different club.");

    public static readonly Error InsufficientRemainingBudget =
        Error.Conflict("InsufficientRemainingBudget", "Insufficient remaining budget to hire the Coach.");
}

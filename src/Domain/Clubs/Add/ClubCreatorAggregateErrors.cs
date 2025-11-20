using SharedKernel;

namespace Domain.Clubs.Add;

public static class ClubCreatorAggregateErrors
{
    public static readonly Error InvalidSpecificationValue =
        Error.NotFound("InvalidSpecificationValue", "Club name o three letter name can not be null or empty.");

    public static readonly Error ClubAlreadyExist =
        Error.NotFound("ClubAlreadyExist", "Club with name o three letter name already exists.");

    public static readonly Error MinBudgetNotReached =
        Error.Invalid("MinBudgetNotReached",
            $"Club does not meet minimum budget requirement of {Club.MinimumAnualBudget}.");
}

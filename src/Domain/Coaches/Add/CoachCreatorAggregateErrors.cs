using SharedKernel;

namespace Domain.Coaches.Add;

public static class CoachCreatorAggregateErrors
{
    public static readonly Error NotEnoughExperience =
        Error.Invalid("NotEnoughExperience", "Coach's experience does not meet minimum required.");
}

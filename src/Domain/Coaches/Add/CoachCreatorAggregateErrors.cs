using SharedKernel;

namespace Domain.Coaches.Add;

public static class CoachCreatorAggregateErrors
{
    public readonly static Error NotEnoughExperience =
        Error.Invalid("NotEnoughExperience", "Coach's experience does not meet minimum required.");
}

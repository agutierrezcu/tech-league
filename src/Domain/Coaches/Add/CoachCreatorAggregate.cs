using Domain.DDD;
using SharedKernel;
using Errors = Domain.Coaches.Add.CoachCreatorAggregateErrors;

namespace Domain.Coaches.Add;

public sealed class CoachCreatorAggregate(Coach root) : AggregateRoot<Coach, CoachId>
{
    protected override Coach Root
        => root ?? throw new InvalidOperationException("Root aggregate can not be null");

    public CoachId CoachId => Root.Id;

    public Result CreateCoach(string fullName, int experience)
    {
        if (experience < Coach.MinimumExperience)
        {
            return Result.Failure<CoachId>(Errors.NotEnoughExperience);
        }

        Root.FullName = fullName;
        Root.Experience = experience;

        return Result.Success();
    }
}

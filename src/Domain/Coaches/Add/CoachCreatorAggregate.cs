using Domain.DDD;
using SharedKernel;

namespace Domain.Coaches.Add;

public sealed class CoachCreatorAggregate : Aggregate<Coach>
{
    protected override Coach Root
        => _root ?? throw new InvalidOperationException("Root aggregate can not be null");

    private Coach _root;

    public CoachId CoachId => Root.Id;

    public Result CreateCoach(string fullName, int experience)
    {
        if (experience < Coach.MinExperience)
        {
            return Result.Failure<CoachId>(
                CoachCreatorAggregateErrors.NotEnoughExperience);
        }

        _root = new()
        {
            FullName = fullName,
            Experience = experience
        };

        return Result.Success();
    }
}

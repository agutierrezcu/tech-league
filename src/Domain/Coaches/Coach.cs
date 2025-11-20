using Domain.DDD;
using StronglyTypedIds;

namespace Domain.Coaches;

[StronglyTypedId]
public readonly partial struct CoachId
{
}

public sealed class Coach : Entity<CoachId>
{
    public const int MinimumExperience = 5;

    public required string FullName { get; set; }

    public required int Experience { get; set; }

    public CoachContract? CurrentContract { get; set; }

    public bool IsAvailable => CurrentContract is null;

    public bool IsCoachingTo(ClubId clubId)
    {
        return CurrentContract?.ClubId == clubId;
    }
}

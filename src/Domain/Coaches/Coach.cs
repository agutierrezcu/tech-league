using System.Diagnostics;
using SharedKernel;
using StronglyTypedIds;

namespace Domain.Coaches;

[StronglyTypedId]
public readonly partial struct CoachId : IStronglyTyped<Guid>
{
}

[DebuggerDisplay("ID: {Id}")]
public sealed class Coach 
{
    public const int MinExperience = 5;

    public CoachId Id { get; set; }

    public string FullName { get; set; }
    
    public int Experience { get; set; }

    public CoachContract? CurrentContract { get; set; }

    public bool IsAvailable => CurrentContract is null;

    public bool IsCoachingTo(ClubId clubId) 
        => CurrentContract?.ClubId == clubId;
}

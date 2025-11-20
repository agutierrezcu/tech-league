using Domain.DDD;
using StronglyTypedIds;

namespace Domain.Clubs;

[StronglyTypedId]
public readonly partial struct ClubId
{
}

public sealed class Club : Entity<ClubId>
{
    public const decimal MinimumAnualBudget = 1000000m;

    public required string Name { get; set; }

    public required string ThreeLettersName { get; set; }

    public required decimal AnualBudget { get; set; }

    public List<Contract> Contracts { get; set; } = [];
}

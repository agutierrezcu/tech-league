using System.Diagnostics;
using SharedKernel;
using StronglyTypedIds;

namespace Domain.Clubs;

[StronglyTypedId]
public readonly partial struct ClubId : IStronglyTyped<Guid>
{
}

[DebuggerDisplay("ID: {Id}")]
public sealed class Club 
{
    public const decimal LeagueMinimumBudgetRequired = 999999m;

    public ClubId Id { get; set; }

    public string Name { get; set; }

    public string ThreeLettersName { get; set; }

    public decimal AnualBudget { get; set; }

    public List<Contract> Contracts { get; set; } = [];
}

using Domain.DDD;

namespace Application.Clubs.Projections;

public sealed record ClubFinanceStatusProjection : IProjection<ClubFinanceStatusProjection>
{
    public required ClubId ClubId { get; set; }

    public Club Club { get; set; }

    public required decimal AnualBudget { get; set; }

    public decimal CommittedInPlayers { get; set; }

    public decimal CommittedInCoaches { get; set; }

    public int PlayerContractCount { get; set; }

    public int CoachContractCount { get; set; }

    public decimal RemainingAnualBudget => AnualBudget - (CommittedInPlayers + CommittedInCoaches);

    public decimal CommittedAnualBudget => CommittedInPlayers + CommittedInCoaches;

    public decimal TotalContractsCount => PlayerContractCount + CoachContractCount;
}

namespace Application.Clubs.Projections.FinanceStatus.GetAll;

public sealed record FinanceStatus
{
    public required ClubId ClubId { get; set; }

    public required string ClubName { get; set; }

    public required decimal AnualBudget { get; set; }

    public decimal CommittedInPlayers { get; set; }

    public decimal CommittedInCoaches { get; set; }

    public int PlayerContractCount { get; set; }

    public int CoachContractCount { get; set; }

    public decimal RemainingAnualBudget { get; set; }

    public decimal CommittedAnualBudget { get; set; }

    public decimal TotalContractCount { get; set; }
}

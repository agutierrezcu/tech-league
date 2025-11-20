namespace Web.Api.Clubs.Finances;

public sealed record GetClubsFinanceStatusResponse
{
    public required ClubId ClubId { get; set; }
    
    public required string ClubName { get; set; }

    public required decimal AnualBudget { get; set; }

    public decimal CommittedInPlayers { get; set; }

    public decimal CommittedInCoaches { get; set; }

    public int PlayerContractCount { get; set; }

    public int CoachContractCount { get; set; }

    public decimal RemainingAnualBudget => AnualBudget - CommittedAnualBudget;

    public decimal CommittedAnualBudget => CommittedInPlayers + CommittedInCoaches;
    
    public decimal TotalContractCount => PlayerContractCount + CoachContractCount;
}


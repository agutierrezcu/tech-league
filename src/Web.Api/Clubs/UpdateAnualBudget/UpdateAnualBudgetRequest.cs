using System.Text.Json.Serialization;

namespace Web.Api.Clubs.UpdateAnualBudget;

internal sealed record UpdateAnualBudgetRequest(ClubId ClubId)
{
    [JsonPropertyName("newValue")]
    public decimal NewAnualBudget { get; set; }
}

using Domain.ValueObjects;

namespace Web.Api.Clubs.Players.SignUp;

internal sealed record SignUpPlayerRequest(ClubId ClubId, PlayerId PlayerId,
    DateRange ContractDuration, decimal AnualSalary);

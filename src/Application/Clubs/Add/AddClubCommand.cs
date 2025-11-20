using Application.Abstractions.Messaging;

namespace Application.Clubs.Add;

public sealed record AddClubCommand(string Name, string ThreeLettersName, decimal AnualBudget)
    : ICommand<ClubId>;

using Application.Abstractions.Messaging;

namespace Application.Clubs.UpdateAnualBudget;

public sealed record UpdateAnualBudgetCommand(ClubId ClubId, decimal NewAnualBudget)
    : ICommand;

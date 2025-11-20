using Application.Abstractions.Messaging;

namespace Application.Clubs.Projections.FinanceStatus.UpdateAnualBudget;

internal sealed record UpdateAnualBudgetFinanceStatusProjectionCommand(ClubId ClubId) : ICommand;

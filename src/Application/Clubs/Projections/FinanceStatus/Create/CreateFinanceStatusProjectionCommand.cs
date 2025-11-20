using Application.Abstractions.Messaging;

namespace Application.Clubs.Projections.FinanceStatus.Create;

internal sealed record CreateFinanceStatusProjectionCommand(ClubId ClubId) : ICommand;

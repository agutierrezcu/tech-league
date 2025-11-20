using Application.Abstractions.Messaging;

namespace Application.Clubs.Projections.FinanceStatus.Update;

internal sealed record UpdateFinanceStatusProjectionCommand
    (ClubId ClubId, decimal UpdateIn, ContractType ContractType)
        : ICommand;

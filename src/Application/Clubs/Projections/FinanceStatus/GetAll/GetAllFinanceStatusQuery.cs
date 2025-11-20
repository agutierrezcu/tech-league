using Application.Abstractions.Messaging;

namespace Application.Clubs.Projections.FinanceStatus.GetAll;

public sealed record GetAllFinanceStatusQuery : IQuery<FinanceStatus[]>;

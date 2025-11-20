using Application.Abstractions;
using Application.Clubs.Get;
using Application.Clubs.Players.Get;
using Application.Clubs.Projections;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

internal sealed class TechLeagueQueryableDbContext
    (IDbContextFactory<TechLeagueDbContext> techLeagueDbContextFactory)
        : QueryableDbContext<TechLeagueDbContext>(techLeagueDbContextFactory),
            IGetClubQueryable,
            IGetPlayersByClubQueryable,
            IClubFinanceStatusQueryable,
            IGetClubCommittedAnualBudgetQueryable
{
    public async Task<PaginatedResult<PlayerByClub>?> GetPlayersByClubAsync(ClubId clubId, string? filterByFullName,
        int pageIndex, int pageSize, CancellationToken ct)
    {
        var queryable = AsNoTracking<Club>()
            .Where(c => c.Id == clubId)
            .LeftJoin(
                AsNoTracking<Contract>().OfType<PlayerContract>(),
                club => club.Id,
                contract => contract.ClubId,
                (club, contract) => new
                {
                    Club = club,
                    Player = contract == null ? null : contract.Player
                })
            .GroupBy(o => o.Club.Id);

        var totalCountResult = await queryable
            .TagWith($"{nameof(GetPlayersByClubAsync)}-TotalCount")
            .Select(g => new
            {
                TotalCount = g
                        .Select(d => d.Player)
                        .Where(p => p != null)
                        .Count(p => filterByFullName == null ||
                            p!.FullName.ToLower().Contains(filterByFullName))
            })
            .FirstOrDefaultAsync(ct);

        if (totalCountResult is null)
        {
            return null;
        }

        if (totalCountResult.TotalCount == 0)
        {
            return new()
            {
                TotalCount = 0,
                TotalPages = 0,
                PageIndex = 1,
                PageSize = pageSize,
                Data = []
            };
        }

        int totalCount = totalCountResult.TotalCount;
        int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        int actualPageIndex = pageIndex;

        if (totalPages == 0)
        {
            actualPageIndex = 1;
        }
        else if (pageIndex > totalPages && totalPages > 0)
        {
            actualPageIndex = totalPages;
        }

        return await queryable
            .TagWith($"{nameof(GetPlayersByClubAsync)}-PaginatedResult")
            .Select(
                g => new PaginatedResult<PlayerByClub>
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    PageIndex = actualPageIndex,
                    PageSize = pageSize,
                    Data = g.Select(d => d.Player)
                        .Where(p => p != null)
                        .Where(p => filterByFullName == null || p!.FullName.ToLower().Contains(filterByFullName))
                        .OrderBy(p => p!.FullName)
                        .Skip((actualPageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .Select(p => new PlayerByClub(p!.Id, p.FullName, p.NickName, p.BirthDate))
                        .ToArray()
                })
            .FirstOrDefaultAsync(ct);
    }

    public Task<Club?> GetClubAsync(ClubId clubId, CancellationToken ct)
    {
        return AsNoTracking<Club>()
            .TagWith($"{nameof(GetClubAsync)}")
            .FirstOrDefaultAsync(c => c.Id == clubId, ct);
    }

    public Task<Contract?> GetContractAsync(ContractId contractId, CancellationToken ct)
    {
        return AsNoTracking<Contract>()
            .TagWith($"{nameof(GetContractAsync)}")
            .FirstOrDefaultAsync(c => c.Id == contractId, ct);
    }

    public IQueryable<ClubFinanceStatusProjection> GetAllFinanceStatusProjections()
    {
        return AsNoTracking<ClubFinanceStatusProjection>();
    }

    public Task<ClubCommittedAnualBudgetInfo?> GetAsync(ClubId clubId, CancellationToken ct)
    {
        return AsNoTracking<Club>()
            .TagWith("GetClubCommittedAnualBudgetInfo") 
            .GetFinanceInfoByClubAsync(clubId, ct);
    }
}

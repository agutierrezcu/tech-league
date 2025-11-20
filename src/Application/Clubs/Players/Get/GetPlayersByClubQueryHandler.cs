using System.Globalization;
using Application.Abstractions.Messaging;
using Application.Clubs.Get;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Clubs.Players.Get;

internal sealed class GetPlayersByClubQueryHandler
    (IGetPlayersByClubQueryable playersQueryable, IGetClubQueryable clubQueryable)
       : IQueryHandler<GetPlayerByClubQuery, GetPlayerByClubQueryResponse>
{
    public async Task<Result<GetPlayerByClubQueryResponse>> HandleAsync(GetPlayerByClubQuery query, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(query);

        // TODO: Get pagination logic to repo

        IQueryable<Player> playersQuery = playersQueryable.GetPlayersByClubAsync(query.ClubId, ct);

        bool anyPlayers = await playersQuery.AnyAsync(ct);

        if (!anyPlayers)
        {
            Club? club = await clubQueryable.GetClubAsync(query.ClubId, ct);

            if (club is null)
            {
                return Result.Failure<GetPlayerByClubQueryResponse>(
                    GetPlayerByClubErrors.ClubNotFound);
            }

            return Result.Success(
                new GetPlayerByClubQueryResponse([], 0, 1, query.PageSize, 0));
        }

        if (!string.IsNullOrWhiteSpace(query.FilterByName))
        {
            string filterValue = query.FilterByName.ToLower(CultureInfo.CurrentCulture);

            playersQuery = playersQuery.Where(
                c => c.FullName.ToLower().Contains(filterValue) ||
                        (c.NickName ?? string.Empty).ToLower().Contains(filterValue));
        }

        int totalCount = await playersQuery.CountAsync(ct);
        int totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

        int pageIndex = query.PageIndex;

        if (totalPages == 0)
        {
            pageIndex = 1;
        }
        else if (query.PageIndex > totalPages && totalPages > 0)
        {
            pageIndex = totalPages;
        }

        PlayerByClub[] players = await playersQuery
            .Select(
                c => new PlayerByClub(c.Id, c.FullName,
                    c.NickName, c.BirthDate))
            .Skip((pageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToArrayAsync(ct);

        return Result.Success(
            new GetPlayerByClubQueryResponse(players, totalCount, pageIndex, 
            query.PageSize, totalPages));
    }
}

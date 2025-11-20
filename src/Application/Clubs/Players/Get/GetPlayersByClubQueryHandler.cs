using Application.Abstractions;
using Application.Abstractions.Messaging;
using SharedKernel;
using Errors = Application.Clubs.Players.Get.GetPlayerByClubErrors;

namespace Application.Clubs.Players.Get;

internal sealed class GetPlayersByClubQueryHandler
    (IGetPlayersByClubQueryable playersQueryable)
       : IQueryHandler<GetPlayerByClubQuery, PaginatedResult<PlayerByClub>>
{
    public async Task<Result<PaginatedResult<PlayerByClub>>> HandleAsync(GetPlayerByClubQuery query, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(query);

        PaginatedResult<PlayerByClub>? paginationResult = await playersQueryable.GetPlayersByClubAsync(
            query.ClubId, query.FilterByName, query.PageIndex, query.PageSize, ct);

        if (paginationResult is null)
        {
            return Result.Failure<PaginatedResult<PlayerByClub>>(Errors.ClubNotFound);
        }

        return paginationResult;
    }
}

using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Clubs.Get;

internal sealed class GetClubQueryHandler(IGetClubQueryable queryable)
    : IQueryHandler<GetClubQuery, GetClubQueryResult>
{
    public async Task<Result<GetClubQueryResult>> HandleAsync(GetClubQuery query, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(query);

        Club? club = await queryable.GetClubAsync(query.ClubId, ct);

        if (club is null)
        {
            return Result.Failure<GetClubQueryResult>(
                GetClubQueryErrors.ClubNotFound);
        }

        GetClubQueryResult result = new(club.Name, club.ThreeLettersName, club.AnualBudget);

        return Result.Success(result);
    }
}

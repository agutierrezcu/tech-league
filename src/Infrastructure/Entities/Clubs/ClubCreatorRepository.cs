using Domain.Clubs.Add;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

using static Domain.Clubs.Add.ClubCreatorAggregateErrors;

namespace Infrastructure.Entities.Clubs;

internal sealed class ClubCreatorRepository
    (TechLeagueDbContext dbContext)
        : RootedRepositoryBase
            <(string Name, string ThreeLettersName), ClubCreatorAggregate, Club>
                (dbContext)
{
    protected override async Task<Result<ClubCreatorAggregate>> CreateRootedAggregateAsync(
        TechLeagueDbContext dbContext, (string Name, string ThreeLettersName) specification,
            CancellationToken ct)
    {
        bool doesClubExist = await dbContext.Clubs.AnyAsync(
            c => EF.Functions.Collate(c.Name, "Latin1_General_CI_AS") == specification.Name ||
                 EF.Functions.Collate(c.Name, "Latin1_General_CI_AS") == specification.ThreeLettersName,
            ct);

        if (doesClubExist)
        {
            return Result.Failure<ClubCreatorAggregate>(ClubAlreadyExist);
        }

        Club root = new()
        {
            Name = specification.Name,
            ThreeLettersName = specification.ThreeLettersName,
            AnualBudget = 0m
        };

        return Result.Success(new ClubCreatorAggregate(root));
    }

    protected override async Task PerformOnRootEntityAsync(TechLeagueDbContext dbContext,
        Club root, CancellationToken ct)
    {
        await dbContext.Clubs.AddAsync(root, ct);
    }
}

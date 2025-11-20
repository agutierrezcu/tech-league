using Domain.Clubs.Add;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

using Errors = Domain.Clubs.Add.ClubCreatorAggregateErrors;

namespace Infrastructure.Entities.Clubs;

internal sealed class ClubCreatorRepository
    (TechLeagueDbContext dbContext)
        : RootedRepositoryBase
            <(string Name, string ThreeLettersName), ClubCreatorAggregate, Club, ClubId>
                (dbContext)
{
    protected override async Task<Result<ClubCreatorAggregate>> CreateRootedAggregateAsync(
        TechLeagueDbContext dbContext, (string Name, string ThreeLettersName) specification,
            CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(specification.Name) ||
                string.IsNullOrWhiteSpace(specification.ThreeLettersName))
        {
            return Result.Failure<ClubCreatorAggregate>(Errors.InvalidSpecificationValue);
        }

        bool doesClubExist = await dbContext.Clubs.AnyAsync(
            c => EF.Functions.Collate(c.Name, "Latin1_General_CI_AS") == specification.Name ||
                 EF.Functions.Collate(c.ThreeLettersName, "Latin1_General_CI_AS") == specification.ThreeLettersName,
            ct);

        if (doesClubExist)
        {
            return Result.Failure<ClubCreatorAggregate>(Errors.ClubAlreadyExist);
        }

        Club root = new()
        {
            Name = specification.Name,
            ThreeLettersName = specification.ThreeLettersName,
            AnualBudget = decimal.MinusOne
        };

        ClubCreatorAggregate aggregate = new(root);

        return Result.Success(aggregate);
    }

    protected override Task PerformOnRootEntityAsync(TechLeagueDbContext dbContext,
        Club root, CancellationToken ct)
    {
        return dbContext.Clubs.AddAsync(root, ct).AsTask();
    }
}

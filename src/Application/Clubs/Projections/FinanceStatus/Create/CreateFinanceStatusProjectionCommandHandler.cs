using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Clubs.Projections.FinanceStatus.Create;

internal sealed class CreateFinanceStatusProjectionCommandHandler
    (IClubFinanceStatusRepository repository)
        : ICommandHandler<CreateFinanceStatusProjectionCommand>
{
    public async Task<Result> HandleAsync(CreateFinanceStatusProjectionCommand command,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(command);

        Club? club = await repository.GetClubAsync(command.ClubId, ct);

        if (club is null)
        {
            return Result.Failure(ClubFinanceStatusProjectionErrors.ClubNotFound);
        }

        ClubFinanceStatusProjection projection = new()
        {
            ClubId = club.Id,
            AnualBudget = club.AnualBudget
        };

        await repository.AddAsync(projection, ct);

        await repository.SaveAsync(ct);

        return Result.Success();
    }
}

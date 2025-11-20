using Application.Abstractions.EventBus;
using Application.Abstractions.Messaging;
using Application.Clubs.Get;
using Domain.DomainEvents;
using SharedKernel;
using Errors = Application.Clubs.Projections.FinanceStatus.ClubFinanceStatusProjectionErrors;

namespace Application.Clubs.Projections.FinanceStatus.Create;

internal sealed class CreateFinanceStatusProjectionCommandHandler
    (IGetClubQueryable queryable, ICreateFinanceStatusProjectionRepository repository, 
        IEventBus eventBus)
            : ICommandHandler<CreateFinanceStatusProjectionCommand>
{
    public async Task<Result> HandleAsync(CreateFinanceStatusProjectionCommand command,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(command);

        Club? club = await queryable.GetClubAsync(command.ClubId, ct);

        if (club is null)
        {
            return Result.Failure(Errors.ClubNotFound);
        }

        ClubFinanceStatusProjection projection = new()
        {
            ClubId = club.Id,
            AnualBudget = club.AnualBudget
        };

        await repository.AddAsync(projection, ct);

        await repository.SaveAsync(ct);

        await eventBus.PublishAsync([new FinanceStatusUpdatedDomainEvent()], ct);

        return Result.Success();
    }
}

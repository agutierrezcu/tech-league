using Application.Abstractions.DependencyInjection;
using Application.Clubs.Projections.FinanceStatus;
using Application.Clubs.Projections.FinanceStatus.Create;
using Application.Clubs.Projections.FinanceStatus.Update;
using Application.Clubs.Projections.FinanceStatus.UpdateAnualBudget;
using Domain.DomainEvents;
using SharedKernel;

namespace Application.Clubs.Projections;

internal sealed class ClubFinanceStatusDomainEventHandler
    (ICommandHandlerMediator mediator, IClubFinanceStatusRepository repository)
        : IDomainEventHandler<NewClubCreatedDomainEvent>,
            IDomainEventHandler<PlayerSignedUpDomainEvent>,
            IDomainEventHandler<CoachHiredDomainEvent>,
            IDomainEventHandler<PlayerReleasedDomainEvent>,
            IDomainEventHandler<CoachDismissedDomainEvent>,
            IDomainEventHandler<ClubAnualBudgetUpdatedDomainEvent>
{
    public async Task HandleAsync(NewClubCreatedDomainEvent domainEvent, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        CreateFinanceStatusProjectionCommand command = new(domainEvent.ClubId);

        Result result = await mediator.MediateAsync(command, ct);

        if (result.IsFailure)
        {
            // log error
        }
    }

    public async Task HandleAsync(PlayerSignedUpDomainEvent domainEvent, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        Contract? contract = await repository.GetContractAsync(domainEvent.ContractId, ct);

        if (contract is null)
        {
            // log error

            return;
        }

        UpdateFinanceStatusProjectionCommand command = new(
            contract.ClubId, contract.AnualSalary, ContractType.Player);

        Result result = await mediator.MediateAsync(command, ct);

        if (result.IsFailure)
        {
            // log error
        }
    }

    public async Task HandleAsync(CoachHiredDomainEvent domainEvent, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        Contract? contract = await repository.GetContractAsync(domainEvent.ContractId, ct);

        if (contract is null)
        {
            // log error

            return;
        }

        UpdateFinanceStatusProjectionCommand command = new(
            contract.ClubId, contract.AnualSalary, ContractType.Coach);

        Result result = await mediator.MediateAsync(command, ct);

        if (result.IsFailure)
        {
            // do   
        }
    }

    public async Task HandleAsync(PlayerReleasedDomainEvent domainEvent, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        UpdateFinanceStatusProjectionCommand command = new(
            domainEvent.ClubId, domainEvent.AnualSalary * -1, ContractType.Player);

        Result result = await mediator.MediateAsync(command, ct);

        if (result.IsFailure)
        {
            // log error
        }
    }

    public async Task HandleAsync(CoachDismissedDomainEvent domainEvent, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        UpdateFinanceStatusProjectionCommand command = new(
            domainEvent.ClubId, domainEvent.AnualSalary * -1, ContractType.Coach);

        Result result = await mediator.MediateAsync(command, ct);

        if (result.IsFailure)
        {
            // log error
        }
    }

    public async Task HandleAsync(ClubAnualBudgetUpdatedDomainEvent domainEvent, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        UpdateAnualBudgetFinanceStatusProjectionCommand command = new(domainEvent.ClubId);

        Result result = await mediator.MediateAsync(command, ct);

        if (result.IsFailure)
        {
            // log error
        }
    }
}

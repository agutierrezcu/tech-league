using Application.Abstractions.DependencyInjection;
using Application.Clubs.Projections.FinanceStatus.Create;
using Application.Clubs.Projections.FinanceStatus.Update;
using Application.Clubs.Projections.FinanceStatus.UpdateAnualBudget;
using Domain.DomainEvents;
using Microsoft.Extensions.Caching.Hybrid;
using SharedKernel;

namespace Application.Clubs.Projections;

internal sealed class ClubFinanceStatusDomainEventHandler
    (ICommandHandlerMediator mediator, IClubFinanceStatusQueryable queryable,
        HybridCache hybridCache)
            : IDomainEventHandler<NewClubCreatedDomainEvent>,
                IDomainEventHandler<PlayerSignedUpDomainEvent>,
                IDomainEventHandler<CoachHiredDomainEvent>,
                IDomainEventHandler<PlayerReleasedDomainEvent>,
                IDomainEventHandler<CoachDismissedDomainEvent>,
                IDomainEventHandler<ClubAnualBudgetUpdatedDomainEvent>,
                IDomainEventHandler<FinanceStatusUpdatedDomainEvent>
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

        Contract? contract = await queryable.GetContractAsync(domainEvent.ContractId, ct);

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

        Contract? contract = await queryable.GetContractAsync(domainEvent.ContractId, ct);

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

        UpdateAnualBudgetProjectionCommand command = new(domainEvent.ClubId);

        Result result = await mediator.MediateAsync(command, ct);

        if (result.IsFailure)
        {
            // log error
        }
    }

    public Task HandleAsync(FinanceStatusUpdatedDomainEvent domainEvent, CancellationToken ct)
    {
        return hybridCache.RemoveAsync("all-finance-status", ct).AsTask();
    }
}

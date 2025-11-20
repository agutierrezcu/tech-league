using Domain.DomainEvents;
using SharedKernel;

namespace Application.Notifications;

internal sealed class EmailNotificationSendingDomainEventHandler
        : IDomainEventHandler<PlayerSignedUpDomainEvent>,
            IDomainEventHandler<PlayerReleasedDomainEvent>
{
    public Task HandleAsync(PlayerReleasedDomainEvent domainEvent, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        // Send notification to anyone interested in

        return Task.CompletedTask;
    }

    public Task HandleAsync(PlayerSignedUpDomainEvent domainEvent, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        // Send notification to anyone interested in

        return Task.CompletedTask;
    }
}

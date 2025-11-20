using SharedKernel;

namespace Application.Abstractions.EventBus;

public interface IEventBus
{
    Task PublishAsync(IEnumerable<IDomainEvent> events, CancellationToken ct);
}

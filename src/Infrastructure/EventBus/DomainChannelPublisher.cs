using System.Threading.Channels;
using SharedKernel;

namespace Infrastructure.EventBus;

public sealed class DomainChannelPublisher(ChannelWriter<IDomainEvent> writer)
    : IEventBusPublisher
{
    public async ValueTask<bool> TryPublishAsync(IDomainEvent @event, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(@event);

        ValueTask writeTask = writer.WriteAsync(@event, ct);
        await writeTask;
        return writeTask.IsCompletedSuccessfully;
    }
}

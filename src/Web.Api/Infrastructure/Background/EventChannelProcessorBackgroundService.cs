using System.Threading.Channels;
using SharedKernel;

namespace Web.Api.Infrastructure.Background;

public sealed class EventChannelProcessorBackgroundService
    (ChannelReader<IDomainEvent> reader, IServiceProvider serviceProvider,
        ILogger<EventChannelProcessorBackgroundService> logger)
            : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        try
        {
            await foreach (IDomainEvent domainEvent in reader.ReadAllAsync(ct))
            {
                Type domainEventType = domainEvent.GetType();

                try
                {
                    Type domainBroadcasterType = typeof(IDomainEventBroadcaster<>)
                        .MakeGenericType(domainEventType);

                    foreach (IDomainEventBroadcaster broadcaster in
                            serviceProvider.GetServices(domainBroadcasterType)
                                .Cast<IDomainEventBroadcaster>())
                    {
                        await broadcaster.PublishAsync(domainEvent, ct);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An unexpected error occurred processing event of type {DomainEventType}",
                        domainEventType);
                }
            }

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error ocurred comsuming published messages");
        }
    }
}

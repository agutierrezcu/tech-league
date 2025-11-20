using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SharedKernel;
using Microsoft.Extensions.Hosting;
using Infrastructure.EventBus;

namespace Infrastructure.Background;

public sealed class DomainEventChannelProcessorBackgroundService
    (ChannelReader<IDomainEvent> reader, IServiceProvider serviceProvider,
        ILogger<DomainEventChannelProcessorBackgroundService> logger)
            : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await foreach (IDomainEvent domainEvent in reader.ReadAllAsync(stoppingToken))
            {
                Type domainEventType = domainEvent.GetType();

                try
                {
                    Type domainBroadcasterType = typeof(IDomainEventBroadcaster<>)
                        .MakeGenericType(domainEventType);

                    IEnumerable<IDomainEventBroadcaster> broadcasters = serviceProvider
                        .GetServices(domainBroadcasterType)
                        .Cast<IDomainEventBroadcaster>();

                    foreach (IDomainEventBroadcaster broadcaster in broadcasters)
                    {
                        await broadcaster.PublishAsync(domainEvent, stoppingToken);
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

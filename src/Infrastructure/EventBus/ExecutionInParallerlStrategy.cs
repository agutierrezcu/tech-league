using SharedKernel;

namespace Infrastructure.EventBus;

public sealed class ExecutionInParallerlStrategy : IExecutionStrategy
{
    public async Task AsyncContinueOnException<TDomainEvent>
        (IEnumerable<IDomainEventHandlerScopedWrapper<TDomainEvent>> handlers, TDomainEvent domainEvent,
            CancellationToken ct)
        where TDomainEvent : IDomainEvent
    {
        List<Task> tasks = [];
        List<Exception> exceptions = [];

        foreach (IDomainEventHandlerScopedWrapper<TDomainEvent> handler in handlers)
        {
            try
            {
                tasks.Add(handler.HandleAsync(domainEvent, ct));
            }
            catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException))
            {
                exceptions.Add(ex);
            }
        }

        try
        {
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
        catch (AggregateException ex)
        {
            exceptions.AddRange(ex.Flatten().InnerExceptions);
        }
        catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException))
        {
            exceptions.Add(ex);
        }

        if (exceptions.Any())
        {
            throw new AggregateException(exceptions);
        }
    }
}

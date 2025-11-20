using Application.Abstractions.Messaging;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.Abstractions.Decorators;

public static class LoggingDecorator
{
    public sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        ILogger<CommandHandler<TCommand, TResponse>> logger)
            : ICommandHandler<TCommand, TResponse>
                where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken ct)
        {
            string commandName = typeof(TCommand).Name;

            logger.LogInformation("Processing command {Command}", commandName);

            Result<TResponse> result = await innerHandler.HandleAsync(command, ct);

            if (result.IsSuccess)
            {
                logger.LogInformation("Completed command {Command}", commandName);
            }
            else
            {
                using (logger.BeginScope(new List<KeyValuePair<string, object>>
                {
                    new("Error", result.Error),
                }))
                {
                    logger.LogError("Completed command {Command} with error", commandName);
                }
            }

            return result;
        }
    }

    public sealed class CommandHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        ILogger<CommandHandler<TCommand>> logger)
            : ICommandHandler<TCommand>
                where TCommand : ICommand
    {
        public async Task<Result> HandleAsync(TCommand command, CancellationToken ct)
        {
            string commandName = typeof(TCommand).Name;

            logger.LogInformation("Processing command {Command}", commandName);

            Result result = await innerHandler.HandleAsync(command, ct);

            if (result.IsSuccess)
            {
                logger.LogInformation("Completed command {Command}", commandName);
            }
            else
            {
                using (logger.BeginScope(new List<KeyValuePair<string, object>>
                {
                    new("Error", result.Error),
                }))
                {
                    logger.LogError("Completed command {Command} with error", commandName);
                }
            }

            return result;
        }
    }

    public sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler,
        ILogger<QueryHandler<TQuery, TResponse>> logger)
            : IQueryHandler<TQuery, TResponse>
                where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> HandleAsync(TQuery query, CancellationToken ct)
        {
            string queryName = typeof(TQuery).Name;

            logger.LogInformation("Processing query {Query}", queryName);

            Result<TResponse> result = await innerHandler.HandleAsync(query, ct);

            if (result.IsSuccess)
            {
                logger.LogInformation("Completed query {Query}", queryName);
            }
            else
            {
                using (logger.BeginScope(new List<KeyValuePair<string, object>>
                {
                    new("Error", result.Error),
                }))
                {
                    logger.LogError("Completed query {Query} with error", queryName);
                }
            }

            return result;
        }
    }

    public sealed class DomainEventHandlerScopedWrapper<TDomainEvent>(
        IDomainEventHandlerScopedWrapper<TDomainEvent> innerHandler,
        ILogger<DomainEventHandlerScopedWrapper<TDomainEvent>> logger)
            : IDomainEventHandlerScopedWrapper<TDomainEvent>
                where TDomainEvent : IDomainEvent
    {
        public Type DomainEventHandlerType => innerHandler.DomainEventHandlerType;

        public async Task HandleAsync(TDomainEvent domainEvent, CancellationToken ct)
        {
            string eventName = typeof(TDomainEvent).Name;

            logger.LogInformation("Processing event {DomainEvent} by {DomainEventHandler}",
                eventName, DomainEventHandlerType.Name);

            await innerHandler.HandleAsync(domainEvent, ct);

            logger.LogInformation("Completed event {DomainEvent} by {DomainEventHandler}",
                eventName, DomainEventHandlerType.Name);
        }
    }
}

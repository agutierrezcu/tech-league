using Application.Abstractions.DependencyInjection;
using Application.Abstractions.Messaging;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel;

namespace Infrastructure.DependencyInjection;

public sealed class CommandHandlerMediator(IServiceScopeFactory serviceScopeFactory)
    : ICommandHandlerMediator
{
    public async Task<Result> MediateAsync<TCommand>(TCommand command, CancellationToken ct)
        where TCommand : ICommand
    {
        await using AsyncServiceScope asyncScope = serviceScopeFactory.CreateAsyncScope();

        ICommandHandler<TCommand> commandHandler = asyncScope.ServiceProvider
            .GetRequiredService<ICommandHandler<TCommand>>();

        return await commandHandler.HandleAsync(command, ct);
    }
}

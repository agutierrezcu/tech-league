using Application.Abstractions.DependencyInjection;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Web.Api.Infrastructure;

public sealed class CommandHandlerMediator(IServiceScopeFactory serviceScopeFactory)
    : ICommandHandlerMediator
{
    public async Task<Result> MediateAsync<TCommand>(TCommand command, CancellationToken ct)
         where TCommand : ICommand
    {
        await using AsyncServiceScope asyncScope = serviceScopeFactory.CreateAsyncScope();

        return await asyncScope.ServiceProvider
            .GetRequiredService<ICommandHandler<TCommand>>()
                .HandleAsync(command, ct);
    }
}

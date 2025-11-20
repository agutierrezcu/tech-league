using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Abstractions.DependencyInjection;

public interface ICommandHandlerMediator
{
    Task<Result> MediateAsync<TCommand>(TCommand command, CancellationToken ct)
        where TCommand : ICommand;
}

using Application.Abstractions.Messaging;
using FluentValidation;
using FluentValidation.Results;
using SharedKernel;

namespace Application.Abstractions.Decorators;

public static class ValidationDecorator
{
    public sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        IEnumerable<IValidator<TCommand>> validators)
            : ICommandHandler<TCommand, TResponse>
                where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken ct)
        {
            ValidationFailure[] validationFailures = await ValidateAsync(command, validators);

            if (validationFailures.Length == 0)
            {
                return await innerHandler.HandleAsync(command, ct);
            }

            return Result.Failure<TResponse>(CreateValidationError(validationFailures));
        }
    }

    public sealed class CommandHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        IEnumerable<IValidator<TCommand>> validators)
            : ICommandHandler<TCommand>
                 where TCommand : ICommand
    {
        public async Task<Result> HandleAsync(TCommand command, CancellationToken ct)
        {
            ValidationFailure[] validationFailures = await ValidateAsync(command, validators);

            if (validationFailures.Length == 0)
            {
                return await innerHandler.HandleAsync(command, ct);
            }

            return Result.Failure(CreateValidationError(validationFailures));
        }
    }

    private static async Task<ValidationFailure[]> ValidateAsync<TCommand>(
        TCommand command,
        IEnumerable<IValidator<TCommand>> validators)
    {
        if (!validators.Any())
        {
            return [];
        }

        var context = new ValidationContext<TCommand>(command);

        ValidationResult[] validationResults = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(context)));

        ValidationFailure[] validationFailures = [.. validationResults
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)];

        return validationFailures;
    }

    public sealed class QueryHandler<TQuery, TResponse>(
      IQueryHandler<TQuery, TResponse> innerHandler,
      IEnumerable<IValidator<TQuery>> validators)
          : IQueryHandler<TQuery, TResponse>
            where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> HandleAsync(TQuery query, CancellationToken ct)
        {
            ValidationFailure[] validationFailures = await ValidateAsync(query, validators);

            if (validationFailures.Length == 0)
            {
                return await innerHandler.HandleAsync(query, ct);
            }

            return Result.Failure<TResponse>(CreateValidationError(validationFailures));
        }
    }

    private static UnprocessableEntityError CreateValidationError(ValidationFailure[] validationFailures)
    {
        return new([.. validationFailures.Select(f => Error.UnprocessableEntity(f.ErrorCode, f.ErrorMessage))]);
    }
}

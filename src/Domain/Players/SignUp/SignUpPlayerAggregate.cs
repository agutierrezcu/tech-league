using Domain.DDD;
using Domain.DomainEvents;
using Domain.ValueObjects;
using SharedKernel;

using static Domain.Players.SignUp.SignUpPlayerAggregateErrors;

namespace Domain.Players.SignUp;

public class SignUpPlayerAggregateRoot(Club club, Player player)
{
    public Club Club => club;

    public Player Player => player;

    public PlayerContract PlayerContract { get; set; }
}

public sealed class SignUpPlayerAggregate
    (SignUpPlayerAggregateRoot root, decimal committedBudget)
        : Aggregate<SignUpPlayerAggregateRoot>
{
    protected override SignUpPlayerAggregateRoot Root
       => root ?? throw new InvalidOperationException("Root aggregate can not be null");

    public Result SignUp(DateRange contractDuration, decimal anualSalary,
         DateRange signingWindow, IDateTimeProvider dateTimeProvider)
    {
        ArgumentNullException.ThrowIfNull(contractDuration);

        if (!signingWindow.Contains(dateTimeProvider.UtcNow))
        {
            return Result.Failure(OutOfSigningWindow);
        }

        if (root.Player.IsPlayingFor(root.Club.Id))
        {
            return Result.Failure(PlayedAlreadySigned);
        }
        else if (!root.Player.IsFreeAgent)
        {
            return Result.Failure(PlayerUnderContractWithDifferentClub);
        }

        decimal remainingBudget = root.Club.AnualBudget - committedBudget;

        if (anualSalary > remainingBudget)
        {
            return Result.Failure(InsufficientRemainingBudget);
        }

        root.PlayerContract = new()
        {
            Club = root.Club,
            Player = root.Player,
            Duration = contractDuration,
            AnualSalary = anualSalary,
        };

        RegisterEvent(
            () => new PlayerSignedUpDomainEvent(root.PlayerContract.Id));

        return Result.Success();
    }
}

using Domain.DDD;
using SharedKernel;
using Domain.DomainEvents;

namespace Domain.Players.RRelease;

public sealed class ReleasePlayerAggregate
    (PlayerContract? contract)
        : Aggregate<PlayerContract>
{
    protected override PlayerContract Root
        => contract ?? throw new InvalidOperationException("Root aggregate can not be null");

    public ValueTask<Result> ReleasePlayer()
    {
        RegisterEvent(
            () => new PlayerReleasedDomainEvent(Root.ClubId, Root.AnualSalary));

        return ValueTask.FromResult(Result.Success());
    }
}

using Domain.DDD;
using Domain.DomainEvents;
using SharedKernel;

namespace Domain.Players.RRelease;

public sealed class ReleasePlayerAggregate(PlayerContract? contract) 
    : AggregateRoot<PlayerContract, ContractId>
{
    protected override PlayerContract Root
        => contract ?? throw new InvalidOperationException("Root aggregate can not be null");

    public ValueTask<Result> ReleasePlayer()
    {
        RegisterEvent(() => new PlayerReleasedDomainEvent(Root.ClubId, Root.AnualSalary));

        return ValueTask.FromResult(Result.Success());
    }
}

using Domain.DDD;
using Domain.DomainEvents;
using SharedKernel;

namespace Domain.Coaches.Dismiss;

public sealed class DismissCoachAggregate(CoachContract contract)
    : AggregateRoot<CoachContract, ContractId>
{
    protected override CoachContract Root
        => contract ?? throw new InvalidOperationException("Root aggregate can not be null");

    public Result CancelContractAsync()
    {
        RegisterEvent(() => new CoachDismissedDomainEvent(Root.ClubId, Root.AnualSalary));

        return Result.Success();
    }
}

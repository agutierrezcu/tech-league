using Domain.DDD;
using Domain.DomainEvents;
using SharedKernel;

namespace Domain.Clubs.Add;

public sealed class ClubCreatorAggregate(Club root) : AggregateRoot<Club, ClubId>
{
    protected override Club Root
        => root ?? throw new InvalidOperationException("Root aggregate can not be null");

    public ClubId ClubId => Root.Id;

    public Result CreateClub(decimal anualBudget)
    {
        if (anualBudget < Club.MinimumAnualBudget)
        {
            return Result.Failure(ClubCreatorAggregateErrors.MinBudgetNotReached);
        }

        Root.AnualBudget = anualBudget;

        RegisterEvent(() => new NewClubCreatedDomainEvent(Root.Id));

        return Result.Success();
    }
}

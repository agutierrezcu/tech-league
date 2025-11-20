using Domain.DDD;
using Domain.ValueObjects;
using StronglyTypedIds;

namespace Domain.Contracts;

[StronglyTypedId]
public readonly partial struct ContractId
{
}

public abstract class Contract : Entity<ContractId>
{
    public required DateRange Duration { get; set; }

    public required decimal AnualSalary { get; set; } = 0;

    public Club Club { get; set; }

    public ClubId ClubId { get; set; }
}

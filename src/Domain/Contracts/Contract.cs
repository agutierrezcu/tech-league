using System.Diagnostics;
using Domain.ValueObjects;
using SharedKernel;
using StronglyTypedIds;

namespace Domain.Contracts;

[StronglyTypedId]
public readonly partial struct ContractId : IStronglyTyped<Guid>
{
}

[DebuggerDisplay("ID: {Id}")]
public abstract class Contract 
{
    public ContractId Id { get; set; }

    public DateRange Duration { get; set; }

    public decimal AnualSalary { get; set; }
    
    public Club Club { get; set; }

    public ClubId ClubId { get; set; }
}

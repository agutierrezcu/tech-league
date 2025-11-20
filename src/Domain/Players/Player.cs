using System.Diagnostics;
using SharedKernel;
using StronglyTypedIds;

namespace Domain.Players;

[StronglyTypedId]
public readonly partial struct PlayerId : IStronglyTyped<Guid>
{
}

[DebuggerDisplay("ID: {Id}")]
public sealed class Player 
{
    public PlayerId Id { get; set; }

    public string FullName { get; set; }
    
    public string? NickName { get; set; }
    
    public DateOnly? BirthDate { get; set; }

    public PlayerContract? CurrentContract { get; set; }
    
    public bool IsFreeAgent => CurrentContract is null;

    public bool IsPlayingFor(ClubId clubId) 
        => CurrentContract?.ClubId == clubId;
}

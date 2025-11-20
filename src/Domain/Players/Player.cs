using Domain.DDD;
using StronglyTypedIds;

namespace Domain.Players;

[StronglyTypedId]
public readonly partial struct PlayerId
{
}

public sealed class Player : Entity<PlayerId>
{
    public required string FullName { get; set; }

    public string? NickName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public PlayerContract? CurrentContract { get; set; }

    public bool IsFreeAgent => CurrentContract is null;

    public bool IsPlayingFor(ClubId clubId)
    {
        return CurrentContract?.ClubId == clubId;
    }
}

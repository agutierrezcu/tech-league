using Domain.DDD;
using SharedKernel;

namespace Domain.Players.Add;

public sealed class PlayerCreatorAggregate(Player root) : AggregateRoot<Player, PlayerId>
{
    protected override Player Root
        => root ?? throw new InvalidOperationException("Root aggregate can not be null");

    public PlayerId PlayerId => Root.Id;

    public Result CreatePlayer(string fullName, string? nickName, DateOnly? birthDate)
    {
        Root.FullName = fullName;
        Root.NickName = nickName;
        Root.BirthDate = birthDate;

        return Result.Success();
    }
}

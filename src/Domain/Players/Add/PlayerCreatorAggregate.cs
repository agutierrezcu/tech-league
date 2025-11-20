using Domain.DDD;
using SharedKernel;

namespace Domain.Players.Add;

public sealed class PlayerCreatorAggregate : Aggregate<Player>
{
    protected override Player Root
        => _root ?? throw new InvalidOperationException("Root aggregate can not be null");

    private Player _root;

    public PlayerId PlayerId => Root.Id;

    public Result CreatePlayer(string fullName, string? nickName, DateOnly? birthDate)
    {
        _root = new()
        {
            FullName = fullName,
            NickName = nickName,
            BirthDate = birthDate
        };

        return Result.Success();
    }
}

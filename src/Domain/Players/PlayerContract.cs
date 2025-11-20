using System.Diagnostics;

namespace Domain.Players;

[DebuggerDisplay("ID: {Id} PlayerId: {PlayerId}")]
public sealed class PlayerContract : Contract
{
    public Player Player { get; set; }

    public PlayerId PlayerId { get; set; }
}

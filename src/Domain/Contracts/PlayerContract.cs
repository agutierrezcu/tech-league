using System.Diagnostics;
using Domain.Players;

namespace Domain.Contracts;

[DebuggerDisplay("ID: {Id} PlayerId: {PlayerId}")]
public sealed class PlayerContract : Contract
{
    public Player Player { get; set; }

    public PlayerId PlayerId { get; set; }
}

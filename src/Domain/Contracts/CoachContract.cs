using System.Diagnostics;
using Domain.Coaches;

namespace Domain.Contracts;

[DebuggerDisplay("ID: {Id} CoachId: {CoachId}")]
public sealed class CoachContract : Contract
{
    public Coach Coach { get; set; }

    public CoachId CoachId { get; set; }
}

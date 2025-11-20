using System.Diagnostics;

namespace Domain.Coaches;

[DebuggerDisplay("ID: {Id} CoachId: {CoachId}")]
public sealed class CoachContract : Contract
{
    public Coach Coach { get; set; }

    public CoachId CoachId { get; set; }
}

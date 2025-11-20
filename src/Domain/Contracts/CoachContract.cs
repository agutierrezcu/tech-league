using System.Diagnostics;
using Domain.Coaches;

namespace Domain.Contracts;

[DebuggerDisplay("ID: {Id} CoachId: {CoachId}")]
public sealed class CoachContract : Contract
{
    public const decimal MinimumAnualSalary = 1000m;

    public Coach Coach { get; set; }

    public CoachId CoachId { get; set; }
}

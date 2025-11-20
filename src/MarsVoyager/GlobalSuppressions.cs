// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S4035:Classes implementing \"IEquatable<T>\" should be sealed", Justification = "<Pending>", Scope = "type", Target = "~T:MarsVoyager.Original.Rover")]
[assembly: SuppressMessage("Style", "IDE0008:Use explicit type", Justification = "<Pending>", Scope = "member", Target = "~M:MarsVoyager.Original.Rover.Receive(System.String)")]
[assembly: SuppressMessage("Globalization", "CA1309:Use ordinal string comparison", Justification = "<Pending>", Scope = "member", Target = "~M:MarsVoyager.Original.Rover.Receive(System.String)")]
[assembly: SuppressMessage("Style", "IDE0045:Convert to conditional expression", Justification = "<Pending>", Scope = "member", Target = "~M:MarsVoyager.Original.Rover.Receive(System.String)")]
[assembly: SuppressMessage("Style", "IDE0011:Add braces", Justification = "<Pending>", Scope = "member", Target = "~M:MarsVoyager.Original.Rover.Equals(System.Object)~System.Boolean")]
[assembly: SuppressMessage("Style", "IDE0041:Use 'is null' check", Justification = "<Pending>", Scope = "member", Target = "~M:MarsVoyager.Original.Rover.Equals(System.Object)~System.Boolean")]
[assembly: SuppressMessage("Minor Bug", "S2328:\"GetHashCode\" should not reference mutable fields", Justification = "<Pending>", Scope = "member", Target = "~M:MarsVoyager.Original.Rover.GetHashCode~System.Int32")]

[assembly: SuppressMessage("Minor Bug", "S2328:\"GetHashCode\" should not reference mutable fields", Justification = "<Pending>", Scope = "member", Target = "~M:MarsVoyager.Rover.Inmutable.GetHashCode~System.Int32")]
[assembly: SuppressMessage("Naming", "CA1708:Identifiers should differ by more than case", Justification = "<Pending>", Scope = "type", Target = "~T:MarsVoyager.Inmutable.CardinalPoint")]
[assembly: SuppressMessage("Naming", "CA1708:Identifiers should differ by more than case", Justification = "<Pending>", Scope = "type", Target = "~T:MarsVoyager.Mutable.CardinalPoint")]
[assembly: SuppressMessage("Naming", "CA1708:Identifiers should differ by more than case", Justification = "<Pending>", Scope = "type", Target = "~T:MarsVoyager.Record.CardinalPoint")]
[assembly: SuppressMessage("Naming", "CA1708:Identifiers should differ by more than case", Justification = "<Pending>", Scope = "type", Target = "~T:MarsVoyager.MutableRecord.CardinalPoint")]

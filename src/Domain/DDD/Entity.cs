using System.Diagnostics;
using SharedKernel;

namespace Domain.DDD;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull, IStronglyTyped<Guid>, new()
{
    public TId Id { get; private set; } = new();

    public override bool Equals(object? obj)
    {
        return Equals(obj as Entity<TId>);
    }

    public bool Equals(Entity<TId>? other)
    {
        return other is not null && other.Id.Equals(Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
        => Equals(left, right);

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
        => !(left == right);

    private string GetDebuggerDisplay()
    {
        return $"{GetType().FullName} Id: {Id}";
    }
}

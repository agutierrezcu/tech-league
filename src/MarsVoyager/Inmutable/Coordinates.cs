using System.Numerics;

using static MarsVoyager.Inmutable.CardinalPoint;
using static MarsVoyager.Direcction;

namespace MarsVoyager.Inmutable;

public readonly struct Coordinates : IEquatable<Coordinates>
{
    private readonly int _x;

    private readonly int _y;

    private Coordinates(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public static Coordinates From(int x, int y) => new(x, y);

    public override readonly int GetHashCode() => HashCode.Combine(_x, _y);

    public readonly bool Equals(Coordinates other) => AreCoordinatesEqual(other);

    public override readonly bool Equals(object? obj)
    {
        return obj switch
        {
            null => false,
            _ => obj is Coordinates other && AreCoordinatesEqual(other)
        };
    }
    private readonly bool AreCoordinatesEqual(Coordinates coordinates)
        => _x == coordinates._x && _y == coordinates._y;

    public override readonly string ToString()
        => $"{nameof(_x)}: {_x}, {nameof(_y)}: {_y}";

    public static bool operator ==(Coordinates a, Coordinates b) => a.Equals(b);

    public static bool operator !=(Coordinates a, Coordinates b) => !(a == b);

    public Coordinates Combine(Coordinates coordinates)
        => From(_x + coordinates._x, _y + coordinates._y);

    public Coordinates Apply(Vector vector, int displacement)
    {
        Func<int, Coordinates> vectorApplier = VectorsMapping[vector];

        return Combine(vectorApplier(displacement));
    }

    private static readonly Dictionary<Vector, Func<int, Coordinates>> VectorsMapping =
        new()
        {
            { new(North, Forward), d => From(0, d) },
            { new(North, Backward), d => From(0, d * -1) },
            { new(South, Forward), d => From(0, d * -1) },
            { new(South, Backward),d => From(0, d) },
            { new(East, Forward), d =>From(d, 0) },
            { new(East, Backward),d => From(d * -1, 0) },
            { new(West, Forward), d =>From(d * -1, 0) },
            { new(West, Backward), d =>From(d, 0 )}
        };
}

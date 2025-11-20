using System.Numerics;

using static MarsVoyager.MutableRecord.CardinalPoint;
using static MarsVoyager.Direcction;

namespace MarsVoyager.MutableRecord;

public sealed record Coordinates
{
    private int _x;

    private int _y;

    private Coordinates(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public static Coordinates From(int x, int y) => new(x, y);

    public override string ToString()
        => $"{nameof(_x)}: {_x}, {nameof(_y)}: {_y}";

    public void Combine(Coordinates coordinates)
    {
        _x += coordinates._x;
        _y += coordinates._y;
    }

    public void Apply(Vector vector, int displacement)
    {
        Func<int, Coordinates> vectorApplier = VectorsMapping[vector];

        Combine(vectorApplier(displacement));
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

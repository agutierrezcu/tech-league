namespace MarsVoyager.Inmutable;

public readonly struct CardinalPoint : IEquatable<CardinalPoint>
{
    public const char NOWHERE = '0';
    public const char NORTH = 'N';
    public const char SOUTH = 'S';
    public const char EAST = 'E';
    public const char WEST = 'W';

    private readonly char _pointingAt;

    private CardinalPoint(char pointingAt)
    {
        _pointingAt = pointingAt;
    }

    public static CardinalPoint Nowhere => new(NOWHERE);

    public static CardinalPoint North => new(NORTH);

    public static CardinalPoint South => new(SOUTH);

    public static CardinalPoint East => new(EAST);

    public static CardinalPoint West => new(WEST);

    public override int GetHashCode() => _pointingAt.GetHashCode();

    public bool Equals(CardinalPoint other) => IsCardinalPointEqual(other);

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            null => false,
            char => Equals(_pointingAt, (char)obj),
            _ => obj is CardinalPoint other && IsCardinalPointEqual(other)
        };
    }
    private bool IsCardinalPointEqual(CardinalPoint? cardinalPoint)
     => Equals(_pointingAt, cardinalPoint?._pointingAt);

    public override string ToString() => _pointingAt.ToString();

    public static bool operator ==(CardinalPoint a, CardinalPoint b) => a.Equals(b);
    public static bool operator !=(CardinalPoint a, CardinalPoint b) => !(a == b);

    public static bool operator ==(CardinalPoint a, char b) => a.Equals(b);
    public static bool operator !=(CardinalPoint a, char b) => !(a == b);

    public static bool operator ==(char a, CardinalPoint b) => b == a;
    public static bool operator !=(char a, CardinalPoint b) => !(b == a);

    private static readonly Dictionary<char, (CardinalPoint Right, CardinalPoint Left)> rotationsMapping =
        new()
        {
            { NORTH, (East, West)},
            { SOUTH, (West, East)},
            { EAST, (South, North)},
            { WEST, (North, South)}
        };

    internal CardinalPoint Rotate(RotationCommand rotation)
    {
        (CardinalPoint Right, CardinalPoint Left) = rotationsMapping[_pointingAt];
        return rotation == RotationCommand.Right ? Right : Left;
    }

    public static CardinalPoint From(char cardinalPoint)
    {
        return cardinalPoint switch
        {
            NORTH => North,
            SOUTH => South,
            EAST => East,
            WEST => West,
            _ => Nowhere
        };
    }

    public bool IsNorth => this == North;

    public bool IsSouth => this == South;

    public bool IsEast => this == East;

    public bool IsWest => this == West;
}

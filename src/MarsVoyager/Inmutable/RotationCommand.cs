namespace MarsVoyager.Inmutable;

public readonly struct RotationCommand : IEquatable<RotationCommand>
{
    private const char NONE = '0';
    private const char LEFT = 'l';
    private const char RIGHT = 'r';

    private readonly char _rotation;

    private RotationCommand(char rotation)
    {
        _rotation = rotation;
    }

    public readonly static RotationCommand None = new(NONE);

    public readonly static RotationCommand Left = new(LEFT);

    public readonly static RotationCommand Right = new(RIGHT);

    public override readonly int GetHashCode() => _rotation.GetHashCode();

    public readonly bool Equals(RotationCommand other)
        => IsRotationEqual(other);

    public override readonly bool Equals(object? obj)
    {
        return obj switch
        {
            null => false,
            char => Equals(_rotation, (char)obj),
            _ => obj is RotationCommand other && IsRotationEqual(other)
        };
    }

    private readonly bool IsRotationEqual(RotationCommand rotateCommand)
        => Equals(_rotation, rotateCommand._rotation);

    public override readonly string ToString() => _rotation.ToString();

    public static bool operator ==(RotationCommand a, RotationCommand b) => a.Equals(b);
    public static bool operator !=(RotationCommand a, RotationCommand b) => !(a == b);

    public static bool operator ==(RotationCommand a, char b) => a.Equals(b);
    public static bool operator !=(RotationCommand a, char b) => !(a == b);

    public static bool operator ==(char a, RotationCommand b) => b == a;
    public static bool operator !=(char a, RotationCommand b) => !(b == a);

    public readonly bool IsSame(char rotation, out RotationCommand outRotation)
    {
        if (this == rotation)
        {
            outRotation = this;
            return true;
        }

        outRotation = None;
        return false;
    }

    public static bool TryFrom(char command, out RotationCommand rotation)
    {
        return Left.IsSame(command, out rotation) || Right.IsSame(command, out rotation);
    }
}
